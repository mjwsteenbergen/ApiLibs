using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public partial class ContactResult
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("value")]
        public List<Contact> Value { get; set; }
    }

    public partial class Contact
    {
        [JsonPropertyName("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonPropertyName("changeKey")]
        public string ChangeKey { get; set; }

        [JsonPropertyName("categories")]
        public List<object> Categories { get; set; }

        [JsonPropertyName("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonPropertyName("birthday")]
        public DateTimeOffset? Birthday { get; set; }

        [JsonPropertyName("fileAs")]
        public string FileAs { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("initials")]
        public object Initials { get; set; }

        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }

        [JsonPropertyName("nickName")]
        public string NickName { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("yomiGivenName")]
        public string YomiGivenName { get; set; }

        [JsonPropertyName("yomiSurname")]
        public string YomiSurname { get; set; }

        [JsonPropertyName("yomiCompanyName")]
        public object YomiCompanyName { get; set; }

        [JsonPropertyName("generation")]
        public string Generation { get; set; }

        [JsonPropertyName("imAddresses")]
        public List<object> ImAddresses { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonPropertyName("profession")]
        public object Profession { get; set; }

        [JsonPropertyName("businessHomePage")]
        public object BusinessHomePage { get; set; }

        [JsonPropertyName("assistantName")]
        public string AssistantName { get; set; }

        [JsonPropertyName("manager")]
        public string Manager { get; set; }

        [JsonPropertyName("homePhones")]
        public List<string> HomePhones { get; set; }

        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("businessPhones")]
        public List<string> BusinessPhones { get; set; }

        [JsonPropertyName("spouseName")]
        public string SpouseName { get; set; }

        [JsonPropertyName("personalNotes")]
        public string PersonalNotes { get; set; }

        [JsonPropertyName("children")]
        public List<object> Children { get; set; }

        [JsonPropertyName("emailAddresses")]
        public List<EmailAddress> EmailAddresses { get; set; }

        [JsonPropertyName("homeAddress")]
        public PhysicalAddress HomeAddress { get; set; }

        [JsonPropertyName("businessAddress")]
        public PhysicalAddress BusinessAddress { get; set; }

        [JsonPropertyName("otherAddress")]
        public PhysicalAddress OtherAddress { get; set; }
    }
}
