using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class ContactResult
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

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
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("categories")]
        public List<object> Categories { get; set; }

        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonProperty("birthday")]
        public DateTimeOffset? Birthday { get; set; }

        [JsonProperty("fileAs")]
        public string FileAs { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("initials")]
        public object Initials { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("nickName")]
        public string NickName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("yomiGivenName")]
        public string YomiGivenName { get; set; }

        [JsonProperty("yomiSurname")]
        public string YomiSurname { get; set; }

        [JsonProperty("yomiCompanyName")]
        public object YomiCompanyName { get; set; }

        [JsonProperty("generation")]
        public string Generation { get; set; }

        [JsonProperty("imAddresses")]
        public List<object> ImAddresses { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonProperty("profession")]
        public object Profession { get; set; }

        [JsonProperty("businessHomePage")]
        public object BusinessHomePage { get; set; }

        [JsonProperty("assistantName")]
        public string AssistantName { get; set; }

        [JsonProperty("manager")]
        public string Manager { get; set; }

        [JsonProperty("homePhones")]
        public List<string> HomePhones { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("businessPhones")]
        public List<string> BusinessPhones { get; set; }

        [JsonProperty("spouseName")]
        public string SpouseName { get; set; }

        [JsonProperty("personalNotes")]
        public string PersonalNotes { get; set; }

        [JsonProperty("children")]
        public List<object> Children { get; set; }

        [JsonProperty("emailAddresses")]
        public List<EmailAddress> EmailAddresses { get; set; }

        [JsonProperty("homeAddress")]
        public PhysicalAddress HomeAddress { get; set; }

        [JsonProperty("businessAddress")]
        public PhysicalAddress BusinessAddress { get; set; }

        [JsonProperty("otherAddress")]
        public PhysicalAddress OtherAddress { get; set; }
    }
}
