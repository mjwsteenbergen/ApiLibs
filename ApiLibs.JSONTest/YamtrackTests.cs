using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ApiLibs.JSON;
using ApiLibs.JSON.Yamtrack;
using NUnit.Framework;

namespace ApiLibs.JSONTest
{
    /// <summary>
    /// Yamtrack exists only in ApiLibs.JSON, so the parity tests do not cover it. These tests check
    /// the requests it sends and that it reads the example responses of Yamtrack's OpenAPI schema.
    /// </summary>
    [Category("NoInternet")]
    public class YamtrackTests
    {
        private sealed class CannedImplementation : ApiLibs.JSON.ICallImplementation
        {
            public string Response = "";
            public ApiLibs.JSON.Request? Last;

            public override Task<ApiLibs.JSON.RequestResponse> ExecuteRequest(ApiLibs.JSON.Service service, ApiLibs.JSON.Request request)
            {
                Last = request;
                var status = request.ExpectedStatusCode.Match(code => code, codes => codes.First());
                return Task.FromResult(new ApiLibs.JSON.RequestResponse(status, "Fake", request.EndPoint, null, Response, null, request, service));
            }
        }

        private CannedImplementation http = null!;
        private YamtrackService yamtrack = null!;

        [SetUp]
        public void SetUp()
        {
            http = new CannedImplementation();
            yamtrack = new YamtrackService("https://yamtrack.example.com/", "secret");
            var field = typeof(ApiLibs.JSON.Service).GetField("<Implementation>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
            foreach (var service in new ApiLibs.JSON.Service[] { yamtrack, yamtrack.MediaService, yamtrack.ListService })
            {
                field.SetValue(service, http);
            }
        }

        private string Parameters() => string.Join("&", http.Last!.Parameters.Where(p => p.Value != null).Select(p => $"{p.Name}={p.Value}"));

        private string Body() => Martijn.Extensions.JSON.Compat.NewtonsoftCompat.Serialize(http.Last!.Content, ignoreNullValues: true);

        [Test]
        public void ItemIds()
        {
            Assert.That(YamtrackService.ItemId(YamtrackMediaType.Movie, YamtrackSource.Tmdb, "603"), Is.EqualTo("movie/tmdb/603"));
            Assert.That(YamtrackService.ItemId(YamtrackMediaType.Tv, YamtrackSource.Tmdb, "1396", 1), Is.EqualTo("tv/tmdb/1396/1"));
            Assert.That(YamtrackService.ItemId(YamtrackMediaType.Tv, YamtrackSource.Tmdb, "1396", 1, 2), Is.EqualTo("tv/tmdb/1396/1/2"));
            Assert.That(YamtrackService.ItemId(YamtrackMediaType.Manga, YamtrackSource.MangaUpdates, "42"), Is.EqualTo("manga/mangaupdates/42"));
            Assert.Throws<ArgumentException>(() => YamtrackService.ItemId(YamtrackMediaType.Tv, YamtrackSource.Tmdb, "1396", episodeNumber: 2));
        }

        [Test]
        public async Task GetMediaSendsFiltersAndReadsResults()
        {
            http.Response = """
                {"pagination": {"total": 1, "limit": 20, "offset": 0, "next": null, "previous": null}, "results": [{"id": 1, "consumption_id": 1, "item": {"media_id": "100088", "source": "tmdb", "media_type": "tv", "title": "The Last of Us", "image": "https://image.tmdb.org/t/p/original//dmo6TYuuJgaYinXBPjrgG9mB5od.jpg", "season_number": null, "episode_number": null}, "item_id": "tv/tmdb/100088", "parent_id": null, "tracked": true, "created_at": "2026-01-15T15:33:03.209328Z", "score": null, "status": 0, "progress": 0, "progressed_at": null, "start_date": null, "end_date": null, "notes": "", "lists": [{"list_id": 1, "list_item_id": 10}]}]}
                """;

            var media = await yamtrack.MediaService.GetMedia(YamtrackMediaType.Tv, YamtrackStatus.InProgress, sort: "score_desc", exclude: new[] { YamtrackMediaType.BoardGame, YamtrackMediaType.Movie });

            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/"));
            Assert.That(Parameters(), Is.EqualTo("media_type=tv&status=In progress&sort=score_desc&exclude=boardgame,movie"));
            Assert.That(media.Pagination.Total, Is.EqualTo(1));
            var show = media.Results.Single();
            Assert.That(show.ItemId, Is.EqualTo("tv/tmdb/100088"));
            Assert.That(show.Item.Source, Is.EqualTo(YamtrackSource.Tmdb));
            Assert.That(show.Item.MediaType, Is.EqualTo(YamtrackMediaType.Tv));
            Assert.That(show.Status, Is.EqualTo(YamtrackStatus.Planning));
            Assert.That(show.Score, Is.Null);
            Assert.That(show.Lists.Single().ListItemId, Is.EqualTo(10));
        }

        [Test]
        public async Task GetEpisodeReadsDetailsAndConsumptions()
        {
            http.Response = """
                {"id": 3612, "media_id": "12345", "source": "tmdb", "source_url": "https://www.themoviedb.org/tv/12345/season/1/episode/1", "media_type": "episode", "title": "Destination Unknown", "max_progress": 1, "image": "https://image.tmdb.org/t/p/original/9xX4Ycm7vN0Qb4JtCjGfC2iC2nH.jpg", "synopsis": "Van der Valk investigates a disappearance...", "genres": ["Drama"], "score": 7.0, "score_count": 3, "details": {"air_date": "1972-09-13", "episode_number": 1, "season_number": 1, "runtime": 50, "episode_type": "standard", "crew": [], "guest_stars": []}, "related": {}, "item_id": "tv/tmdb/12345/1/1", "parent_id": "tv/tmdb/12345/1", "tracked": true, "consumptions_number": 1, "consumptions": [{"consumption_id": 322, "created": null, "score": null, "progress": 1, "progressed_at": null, "status": 3, "start_date": null, "end_date": "2026-05-11T17:20:38Z", "notes": ""}], "lists": []}
                """;

            var episode = await yamtrack.MediaService.GetEpisode("tv/tmdb/12345/1/1");

            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/tv/tmdb/12345/1/1/"));
            Assert.That(episode.MediaType, Is.EqualTo(YamtrackMediaType.Episode));
            Assert.That(episode.Details.Runtime, Is.EqualTo(50));
            Assert.That(episode.Details.AirDate, Is.EqualTo(new DateTime(1972, 9, 13)));
            Assert.That(episode.Consumptions.Single().Status, Is.EqualTo(YamtrackStatus.Completed));
            Assert.That(episode.Consumptions.Single().EndDate!.Value.ToUniversalTime(), Is.EqualTo(new DateTime(2026, 5, 11, 17, 20, 38, DateTimeKind.Utc)));
        }

        [Test]
        public async Task GetCalendarFormatsDates()
        {
            http.Response = """
                {"pagination": {"total": 1, "limit": 20, "offset": 0, "next": null, "previous": null}, "results": [{"id": 5086, "item": {"media_id": "208569", "source": "tmdb", "media_type": "episode", "title": "Will Trent", "image": "https://image.tmdb.org/t/p/w500/qG5O46gUxxYGImld03tl2zLhvrg.jpg", "season_number": 4, "episode_number": 13}, "item_id": "tv/tmdb/208569/4/13", "parent_id": "tv/tmdb/208569/4", "content_number": 13, "datetime": "2026-04-01T00:00:00Z", "notification_sent": false}]}
                """;

            var calendar = await yamtrack.GetCalendar(new DateTime(2026, 4, 1), new DateTime(2026, 4, 30));

            Assert.That(http.Last!.EndPoint, Is.EqualTo("calendar/"));
            Assert.That(Parameters(), Is.EqualTo("start_date=2026-04-01&end_date=2026-04-30"));
            var release = calendar.Results.Single();
            Assert.That(release.Item.EpisodeNumber, Is.EqualTo(13));
            Assert.That(release.ParentId, Is.EqualTo("tv/tmdb/208569/4"));
        }

        [Test]
        public async Task TrackSendsSourceAsTextAndStatusAsNumber()
        {
            http.Response = """{"id": 7, "item_id": "movie/tmdb/603", "tracked": true, "status": 3, "lists": []}""";

            var movie = await yamtrack.MediaService.Track(YamtrackMediaType.Movie, new YamtrackTrackRequest
            {
                Source = YamtrackSource.Tmdb,
                MediaId = "603",
                Status = YamtrackStatus.Completed,
                Score = 8.5,
            });

            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/movie/"));
            Assert.That(http.Last.Method, Is.EqualTo(ApiLibs.JSON.Call.POST));
            Assert.That(FakeHttp.Expected(http.Last.ExpectedStatusCode), Is.EqualTo(HttpStatusCode.Created));
            Assert.That(Body(), Is.EqualTo("""{"source":"tmdb","media_id":"603","score":8.5,"status":3}"""));
            Assert.That(movie.Id, Is.EqualTo(7));
        }

        [Test]
        public async Task UpdateOnlySendsWhatIsSet()
        {
            http.Response = """{"consumption_id": 12, "status": 1, "progress": 3, "notes": ""}""";

            var entry = await yamtrack.MediaService.UpdateHistoryEntry("anime/mal/1", 12, new YamtrackUpdate { Progress = 3 });

            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/anime/mal/1/history/12/"));
            Assert.That(http.Last.Method, Is.EqualTo(ApiLibs.JSON.Call.PATCH));
            Assert.That(Body(), Is.EqualTo("""{"progress":3}"""));
            Assert.That(entry.Progress, Is.EqualTo(3));
        }

        [Test]
        public async Task DeletesExpectNoContent()
        {
            await yamtrack.MediaService.DeleteItem("movie/tmdb/603");
            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/movie/tmdb/603/"));
            Assert.That(http.Last.Method, Is.EqualTo(ApiLibs.JSON.Call.DELETE));
            Assert.That(FakeHttp.Expected(http.Last.ExpectedStatusCode), Is.EqualTo(HttpStatusCode.NoContent));

            await yamtrack.ListService.RemoveItem(2, 28);
            Assert.That(http.Last!.EndPoint, Is.EqualTo("lists/2/items/28/"));

            await yamtrack.DeleteChange(YamtrackMediaType.Episode, 5);
            Assert.That(http.Last!.EndPoint, Is.EqualTo("changes_history/episode/5/"));
        }

        [Test]
        public async Task ListMembership()
        {
            http.Response = """[{"list_id": 2, "list_item_id": 28}]""";

            var lists = await yamtrack.MediaService.AddToList("tv/tmdb/1396/1", 2);

            Assert.That(http.Last!.EndPoint, Is.EqualTo("media/tv/tmdb/1396/1/lists/2/"));
            Assert.That(http.Last.Method, Is.EqualTo(ApiLibs.JSON.Call.PUT));
            Assert.That(lists.Single().ListItemId, Is.EqualTo(28));
        }

        [Test]
        public async Task CreateList()
        {
            http.Response = """{"id": 3, "name": "Watch with friends", "description": "", "image": null, "owner": {"id": 1, "username": "martijn"}, "collaborators": [], "items_count": 0, "latest_update": null}""";

            var list = await yamtrack.ListService.CreateList("Watch with friends");

            Assert.That(http.Last!.EndPoint, Is.EqualTo("lists/"));
            Assert.That(Body(), Is.EqualTo("""{"name":"Watch with friends"}"""));
            Assert.That(list.Owner.Username, Is.EqualTo("martijn"));
        }

        [Test]
        public async Task SearchUsesTheProvider()
        {
            http.Response = """{"pagination": {"total": 1, "limit": 20, "offset": 0, "next": null, "previous": null}, "results": [{"media_id": "603", "source": "tmdb", "media_type": "movie", "title": "The Matrix", "image": "https://image.tmdb.org/t/p/w500/x.jpg"}]}""";

            var results = await yamtrack.Search(YamtrackMediaType.Movie, "matrix", YamtrackSource.Tmdb, limit: 5);

            Assert.That(http.Last!.EndPoint, Is.EqualTo("search/movie/"));
            Assert.That(Parameters(), Is.EqualTo("search=matrix&source=tmdb&limit=5"));
            Assert.That(results.Results.Single().MediaType, Is.EqualTo(YamtrackMediaType.Movie));
        }

        [Test]
        public async Task UnhealthyInstanceIsNotAnError()
        {
            http.Response = """{"status": "unavailable", "timestamp": "2026-10-26T16:18:18Z", "checks": {"database": {"status": "error", "error": "connection refused"}}}""";

            var health = await yamtrack.GetHealth();

            Assert.That(http.Last!.EndPoint, Is.EqualTo("health/"));
            Assert.That(health.Checks["database"].Error, Is.EqualTo("connection refused"));
        }

        [Test]
        public async Task SeasonsAndDetailsOfAShow()
        {
            http.Response = """
                {"id": null, "media_id": "1396", "source": "tmdb", "media_type": "tv", "title": "Breaking Bad", "max_progress": 62, "details": {"status": "Ended", "tvdb_id": 81189, "first_air_date": "2008-01-20"}, "related": {"seasons": [{"id": null, "item": {"media_id": "1396", "source": "tmdb", "media_type": "season", "title": "Season 1", "image": "x", "season_number": 1, "episode_number": null}, "item_id": "tv/tmdb/1396/1", "parent_id": "tv/tmdb/1396", "tracked": false, "status": null, "lists": []}], "recommendations": []}, "item_id": "tv/tmdb/1396", "tracked": false, "consumptions_number": 0, "consumptions": [], "lists": []}
                """;

            var show = await yamtrack.MediaService.GetItem("tv/tmdb/1396");

            Assert.That(show.Seasons.Single().Item.SeasonNumber, Is.EqualTo(1));
            Assert.That(show.Episodes, Is.Empty);
            Assert.That(show.GetDetail<long?>("tvdb_id"), Is.EqualTo(81189));
            Assert.That(show.GetDetail<string>("status"), Is.EqualTo("Ended"));
            Assert.That(show.GetDetail<DateTime?>("first_air_date"), Is.EqualTo(new DateTime(2008, 1, 20)));
            Assert.That(show.GetDetail<string>("missing"), Is.Null);
        }

        [Test]
        public async Task AllPagesReadsUntilThereIsNoNextPage()
        {
            var offsets = new List<int>();
            var all = await YamtrackService.AllPages((limit, offset) =>
            {
                offsets.Add(offset);
                var results = Enumerable.Range(offset, offset < 400 ? limit : 5).ToList();
                return Task.FromResult(new YamtrackPaginated<int>
                {
                    Pagination = new YamtrackPagination { Next = offset < 400 ? "next" : null },
                    Results = results,
                });
            });

            Assert.That(offsets, Is.EqualTo(new[] { 0, 200, 400 }));
            Assert.That(all.Count, Is.EqualTo(405));
        }
    }
}
