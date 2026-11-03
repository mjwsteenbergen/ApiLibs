using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public partial class PersonResult : ValueResult<Person> { }

    public partial class Person
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("personNotes")]
        public string PersonNotes { get; set; }

        [JsonPropertyName("isFavorite")]
        public bool IsFavorite { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("yomiCompany")]
        public string YomiCompany { get; set; }

        [JsonPropertyName("department")]
        public object Department { get; set; }

        [JsonPropertyName("officeLocation")]
        public object OfficeLocation { get; set; }

        [JsonPropertyName("profession")]
        public string Profession { get; set; }

        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonPropertyName("imAddress")]
        public object ImAddress { get; set; }

        [JsonPropertyName("scoredEmailAddresses")]
        public List<ScoredEmailAddress> ScoredEmailAddresses { get; set; }

        [JsonPropertyName("phones")]
        public List<Phone> Phones { get; set; }

        [JsonPropertyName("postalAddresses")]
        public List<PostalAddress> PostalAddresses { get; set; }

        [JsonPropertyName("websites")]
        public List<object> Websites { get; set; }

        [JsonPropertyName("personType")]
        public object PersonType { get; set; }

        public override string ToString()
        {
            return GivenName ?? DisplayName;
        }
    }

    public partial class PersonType
    {
        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("subclass")]
        public string Subclass { get; set; }
    }

    public partial class Phone
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }
    }

    public partial class PostalAddress
    {
        [JsonPropertyName("locationUri"), NullValueHandling(NullHandling.Ignore)]
        public string LocationUri { get; set; }

        [JsonPropertyName("locationType")]
        public string LocationType { get; set; }

        [JsonPropertyName("uniqueIdType")]
        public string UniqueIdType { get; set; }

        [JsonPropertyName("address")]
        public PhysicalAddress Address { get; set; }

        [JsonPropertyName("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

    public partial class ScoredEmailAddress
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("relevanceScore")]
        public long RelevanceScore { get; set; }

        [JsonPropertyName("selectionLikelihood")]
        public string SelectionLikelihood { get; set; }
    }
}
