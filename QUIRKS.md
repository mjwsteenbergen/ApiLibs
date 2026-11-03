# Newtonsoft.Json quirks in the System.Text.Json port

`ApiLibs.JSON` and `Martijn.Extensions.JSON` reproduce Newtonsoft.Json 12 bug for bug, so that
switching a consumer from `ApiLibs` to `ApiLibs.JSON` changes nothing. The behaviour comes from
the compatibility layer in `Martijn.Extensions.JSON/Compat`.

This file lists behaviour that is arguably wrong but kept on purpose, and the few differences
that could not be reproduced. Once `ApiLibs` is deleted and `ApiLibs.JSON` is renamed, each kept
quirk can be fixed as a separate, reviewed change.

## Reproduced on purpose

Each one is pinned by a test named `KnownNewtonsoftQuirk_*` in
`Martijn.Extensions.JSONTest/KnownNewtonsoftQuirkTests.cs`.

| Quirk | Test |
|---|---|
| A JSON fraction is rounded (banker's rounding) into `long`, `uint`, `ulong`, `ushort` and `sbyte`, but rejected for `int`, `short` and `byte`. | `FractionsAreRoundedIntoLongButRejectedForInt` |
| `true`/`false` become `1`/`0` for `long` and the other `Convert.ChangeType` types, but are rejected for `int`. | `BooleansBecomeNumbersForLongButNotForInt` |
| Enum names separated by commas are OR-ed together even on enums without `[Flags]`; `true` becomes the value `1`; undefined numbers are accepted. | `EnumNamesAreCombinedEvenWithoutFlags` |
| ISO dates with an offset are converted to local time (`DateTimeKind.Local`), losing the original offset. In an `object` property, strings that look like ISO date-times become `DateTime`. | `DateOffsetsAreConvertedToLocalTime` |
| An empty string becomes `null` for objects, lists and nullable values, but is an error for non-nullable numbers. | `EmptyStringIsNullForObjectsButAnErrorForNumbers` |
| `NullValueHandling.Ignore` (now `[NullValueHandling(NullHandling.Ignore)]`) also skips JSON `null` when reading, so the property keeps its default. | `NullValueHandlingIgnoreAlsoIgnoresNullsWhenReading` |
| A `string` property receives the raw text of a number (`1.50` stays `"1.50"`) and `true`/`false` as lowercase text. | `StringsKeepTheRawTextOfNumbersAndBooleans` |
| Whole doubles are written as `1.0`, and NaN/Infinity as strings. | `WholeDoublesAreWrittenWithDecimalPlace` |

## Not reproduced

| Difference | Why | Status |
|---|---|---|
| A property typed `object` receives a `System.Text.Json.JsonElement` for JSON objects and arrays, where Newtonsoft gave a `JObject`/`JArray`. Public members typed `JToken` become `JsonElement`. | `JObject` is a Newtonsoft type, which the new packages cannot reference. | Agreed swap (the API diff allows exactly this). |
| When a JSON object contains two keys that differ only in case (for example `tags` and `Tags`) and one of them matches a `[JsonIgnore]` property exactly, Newtonsoft drops that key; System.Text.Json assigns it to the other property. | System.Text.Json has no per-key exact-match-first lookup. Reproducing it would need a custom converter per affected class. | Needs a decision. No ApiLibs class is known to receive such JSON. |
| A class with two serialized members whose names differ only in case (for example a private `secret` field marked `[JsonProperty]` next to a public `Secret` property) works in Newtonsoft but makes System.Text.Json throw. | System.Text.Json rejects case-insensitive name clashes when case-insensitive matching is on. | Needs a decision if an ApiLibs class has this shape. The generated parity tests fail for any class that does. |
| A settable property that already holds an object (set in the constructor) is filled in place by Newtonsoft, keeping members the JSON does not mention. The new version replaces the object. Lists, dictionaries and read-only properties are filled in place like Newtonsoft does. | Settable object properties go through a property converter (for empty-string and runtime-type handling), and System.Text.Json cannot fill an existing object through a custom converter. | Needs a decision. Differs only when a constructor puts non-default values in a nested object; the generated tests found no ApiLibs class where it changes the result. |
| An empty string for a read-only property that holds an object is skipped by Newtonsoft and is an error in the new version. | Same limitation: read-only objects are filled by System.Text.Json's own converter, which rejects a string. | Needs a decision. No API is known to send this. |
| For a class whose only constructor has a parameter that matches no property (OpenAI's `OpenAIUrlContent(string url)`), Newtonsoft passes a JSON value with the parameter's name to the constructor. The new version always passes null/default and then sets the properties, as Newtonsoft does when that key is absent. | System.Text.Json cannot bind a constructor parameter to a key without a matching property. | Needs a decision. OpenAI responses have no top-level `url` key, so the result is the same. |

## Bugs in ApiLibs itself, kept on purpose

Found while writing the hand-written Notion fixtures (`ApiLibs.JSONTest/HandWrittenParityTests.cs`).
They behave the same in `ApiLibs` and `ApiLibs.JSON`.

| Bug | Fixture |
|---|---|
| A Notion date property whose `start` includes a time (`2023-03-05T09:30:00.000+01:00`) makes the whole page fail with `FormatException`. Newtonsoft has already turned the string into a `DateTime`, `DateTimeConverter` gets it back as `"03/05/2023 09:30:00"`, and its `yyyy-MM-dd` regex does not match. Date-only values work. | `NotionRest.Page (date property with a time ...)` |
| A Notion relation property on a page (an array of ids) cannot be read: `Relation` expects the database-schema shape (an object), so the whole page fails. | `NotionRest.Page (relation values as an array)` |
| A page or database property of a type the converter does not know (`people`, `formula`, `rollup`, `created_by`, ...) makes the whole page fail with `ArgumentOutOfRangeException`. The same goes for unknown block types (`synced_block`, `table`, ...). | `NotionRest.Page (unknown property type)`, `NotionRest.NotionList<NotionBlockWrapper> (unknown block type)` |
| Image, video and pdf blocks (`NotionImage`, which derives from `NotionFileWrapper`) never get their `Value` filled when read, so writing them back throws `NullReferenceException`. | `NotionRest.NotionList<NotionBlockWrapper> (block children with every block type)` |
