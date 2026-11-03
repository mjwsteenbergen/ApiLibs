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
