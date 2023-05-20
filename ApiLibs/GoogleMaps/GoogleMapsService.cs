using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Martijn.Extensions.Linq;

namespace ApiLibs.GoogleMaps
{
    public enum PlaceField {
        AddressComponent,
        AdrAddress,
        BusinessStatus,
        FormattedAddress,
        Geometry,
        Icon,
        IconMaskBaseUri,
        IconBackgroundColor,
        Name,
        PermanentlyClosed,
        Photo,
        PlaceId,
        PlusCode,
        Type,
        Url,
        UtcOffset,
        Vicinity
    }

    public class GoogleMapsService : RestSharpService
    {
        public GoogleMapsService(string token) : base("https://maps.googleapis.com/maps/api/")
        {
            AddStandardParameter("key", token);
        }

        public Task<PlaceSearchResult> FindPlace(string input, List<PlaceField> fields = null) => MakeRequest<PlaceSearchResult>("place/findplacefromtext/json", parameters: new List<Param> {
            new Param("input", input),
            new Param("inputtype", "textquery"),
            new OParam("fields", fields?.Select(i => FieldToString(i)).Combine((i,j) => i + "," + j))
        });

        private string FieldToString(PlaceField field) {
            return field switch
            {
                PlaceField.AddressComponent => "address_component",
                PlaceField.AdrAddress => "adr_address",
                PlaceField.BusinessStatus => "business_status",
                PlaceField.FormattedAddress => "formatted_address",
                PlaceField.Geometry => "geometry",
                PlaceField.Icon => "icon",
                PlaceField.IconMaskBaseUri => "icon_mask_base_uri",
                PlaceField.IconBackgroundColor => "icon_background_color",
                PlaceField.Name => "name",
                PlaceField.PermanentlyClosed => "permanently_closed",
                PlaceField.Photo => "photo",
                PlaceField.PlaceId => "place_id",
                PlaceField.PlusCode => "plus_code",
                PlaceField.Type => "type",
                PlaceField.UtcOffset => "utc_offset",
                PlaceField.Vicinity => "vicinity",
                _ => throw new System.Exception("Invalid Field")
            };
        }
    }
}