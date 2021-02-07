using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class PersonResult : ValueResult<Person> { }

    public partial class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("personNotes")]
        public string PersonNotes { get; set; }

        [JsonProperty("isFavorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("yomiCompany")]
        public string YomiCompany { get; set; }

        [JsonProperty("department")]
        public object Department { get; set; }

        [JsonProperty("officeLocation")]
        public object OfficeLocation { get; set; }

        [JsonProperty("profession")]
        public string Profession { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("imAddress")]
        public object ImAddress { get; set; }

        [JsonProperty("scoredEmailAddresses")]
        public List<ScoredEmailAddress> ScoredEmailAddresses { get; set; }

        [JsonProperty("phones")]
        public List<Phone> Phones { get; set; }

        [JsonProperty("postalAddresses")]
        public List<PostalAddress> PostalAddresses { get; set; }

        [JsonProperty("websites")]
        public List<object> Websites { get; set; }

        [JsonProperty("personType")]
        public object PersonType { get; set; }

        public override string ToString()
        {
            return GivenName ?? DisplayName;
        }
    }

    public partial class PersonType
    {
        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("subclass")]
        public string Subclass { get; set; }
    }

    public partial class Phone
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }

    public partial class PostalAddress
    {
        [JsonProperty("locationUri", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationUri { get; set; }

        [JsonProperty("locationType")]
        public string LocationType { get; set; }

        [JsonProperty("uniqueIdType")]
        public string UniqueIdType { get; set; }

        [JsonProperty("address")]
        public PhysicalAddress Address { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

    public partial class ScoredEmailAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("relevanceScore")]
        public long RelevanceScore { get; set; }

        [JsonProperty("selectionLikelihood")]
        public string SelectionLikelihood { get; set; }
    }
}
