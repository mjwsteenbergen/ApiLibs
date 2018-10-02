using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class ContactRoot
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }

        [JsonProperty("value")]
        public List<Contact> Value { get; set; }
    }

    public partial class Contact
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("categories")]
        public List<object> Categories { get; set; }

        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonProperty("birthday")]
        public object Birthday { get; set; }

        [JsonProperty("fileAs")]
        public string FileAs { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("initials")]
        public object Initials { get; set; }

        [JsonProperty("middleName")]
        public object MiddleName { get; set; }

        [JsonProperty("nickName")]
        public object NickName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("title")]
        public object Title { get; set; }

        [JsonProperty("yomiGivenName")]
        public object YomiGivenName { get; set; }

        [JsonProperty("yomiSurname")]
        public object YomiSurname { get; set; }

        [JsonProperty("yomiCompanyName")]
        public object YomiCompanyName { get; set; }

        [JsonProperty("generation")]
        public object Generation { get; set; }

        [JsonProperty("imAddresses")]
        public List<object> ImAddresses { get; set; }

        [JsonProperty("jobTitle")]
        public object JobTitle { get; set; }

        [JsonProperty("companyName")]
        public object CompanyName { get; set; }

        [JsonProperty("department")]
        public object Department { get; set; }

        [JsonProperty("officeLocation")]
        public object OfficeLocation { get; set; }

        [JsonProperty("profession")]
        public object Profession { get; set; }

        [JsonProperty("businessHomePage")]
        public object BusinessHomePage { get; set; }

        [JsonProperty("assistantName")]
        public object AssistantName { get; set; }

        [JsonProperty("manager")]
        public object Manager { get; set; }

        [JsonProperty("homePhones")]
        public List<object> HomePhones { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("businessPhones")]
        public List<object> BusinessPhones { get; set; }

        [JsonProperty("spouseName")]
        public object SpouseName { get; set; }

        [JsonProperty("personalNotes")]
        public string PersonalNotes { get; set; }

        [JsonProperty("children")]
        public List<object> Children { get; set; }

        [JsonProperty("emailAddresses")]
        public List<EmailAddress> EmailAddresses { get; set; }

        [JsonProperty("homeAddress")]
        public Address HomeAddress { get; set; }

        [JsonProperty("businessAddress")]
        public Address BusinessAddress { get; set; }

        [JsonProperty("otherAddress")]
        public Address OtherAddress { get; set; }
    }

    public partial class EmailAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
