using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// Read, create, and delete highlights on a bookmark.
    /// </summary>
    public class HighlightService : SubService<InstapaperV2Service>
    {
        public HighlightService(InstapaperV2Service service) : base(service) { }

        public async Task<List<InstapaperHighlight>> GetHighlights(long bookmarkId) => (await MakeRequest<InstapaperHighlightList>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/highlights")).Highlights;
        public Task<List<InstapaperHighlight>> GetHighlights(InstapaperBookmark bookmark) => GetHighlights(bookmark.Id);

        /// <summary>
        /// Create a highlight. Accounts without Premium can create five per month; past that the API answers with a 403.
        /// </summary>
        /// <param name="text">The highlighted text. Leading and trailing whitespace is trimmed by the API.</param>
        /// <param name="position">Which occurrence of <paramref name="text"/> in the article body to highlight, counting from 0.</param>
        public Task<InstapaperHighlight> AddHighlight(long bookmarkId, string text, string note = null, int? position = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Highlight text cannot be blank", nameof(text));
            }
            var body = new Dictionary<string, object> { { "text", text } };
            if (note != null)
            {
                body.Add("note", note);
            }
            if (position != null)
            {
                body.Add("position", position.Value);
            }
            return MakeRequest<InstapaperHighlight>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/highlights", Call.POST, content: body);
        }

        public Task<InstapaperHighlight> AddHighlight(InstapaperBookmark bookmark, string text, string note = null, int? position = null) => AddHighlight(bookmark.Id, text, note, position);

        /// <summary>
        /// Delete a highlight. The API answers with a 400 if the highlight doesn't exist, belongs to someone else, or was already deleted.
        /// </summary>
        public Task DeleteHighlight(long highlightId) => MakeRequest($"highlights/{InstapaperV2Service.CheckId(highlightId)}", Call.DELETE);
        public Task DeleteHighlight(InstapaperHighlight highlight) => DeleteHighlight(highlight.Id);
    }
}
