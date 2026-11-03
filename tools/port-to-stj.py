#!/usr/bin/env python3
"""Copy a folder of ApiLibs into ApiLibs.JSON and apply the mechanical part of the
Newtonsoft.Json -> System.Text.Json port. Custom converters and direct JsonConvert calls
are ported by hand afterwards; the build fails until they are, because Newtonsoft.Json is
not available to ApiLibs.JSON.

    tools/port-to-stj.py General [--exclude Passwords.cs]
"""
import argparse
import pathlib
import re
import shutil

ROOT = pathlib.Path(__file__).resolve().parent.parent
SOURCE = ROOT / "ApiLibs"
TARGET = ROOT / "ApiLibs.JSON"

NULL_HANDLING = re.compile(
    r'\[JsonProperty\((?:PropertyName\s*=\s*)?("[^"]*")\s*,\s*NullValueHandling\s*=\s*NullValueHandling\.(Ignore|Include)\)\]')
PROPERTY = re.compile(r'\[JsonProperty\((?:PropertyName\s*=\s*)?("[^"]*")\)\]')


def convert(text: str) -> str:
    # Namespaces: ApiLibs.X -> ApiLibs.JSON.X, Martijn.Extensions.X -> Martijn.Extensions.JSON.X
    text = re.sub(r'\b(namespace|using)\s+ApiLibs\b(?!\.JSON)', r'\1 ApiLibs.JSON', text)
    text = re.sub(r'\b(namespace|using)\s+Martijn\.Extensions\.(?!JSON)', r'\1 Martijn.Extensions.JSON.', text)
    text = re.sub(r'(?<![\w."/])ApiLibs\.(?!JSON\b)(?=[A-Z])', 'ApiLibs.JSON.', text)

    uses_null_handling = bool(NULL_HANDLING.search(text))
    text = NULL_HANDLING.sub(lambda m: f'[JsonPropertyName({m.group(1)}), NullValueHandling(NullHandling.{m.group(2)})]', text)
    text = PROPERTY.sub(r'[JsonPropertyName(\1)]', text)
    text = text.replace('[JsonIgnore()]', '[JsonIgnore]')

    usings = 'using System.Text.Json.Serialization;'
    if uses_null_handling:
        usings += '\nusing Martijn.Extensions.JSON.Compat;'
    text = re.sub(r'^(﻿?)using Newtonsoft\.Json;', lambda m: m.group(1) + usings, text, count=1, flags=re.M)
    return text


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('folder')
    parser.add_argument('--exclude', action='append', default=[])
    args = parser.parse_args()

    source = SOURCE / args.folder
    target = TARGET / args.folder
    if target.exists():
        shutil.rmtree(target)
    for path in sorted(source.rglob('*.cs')):
        relative = path.relative_to(source)
        if relative.name in args.exclude or 'obj' in relative.parts or 'bin' in relative.parts:
            continue
        destination = target / relative
        destination.parent.mkdir(parents=True, exist_ok=True)
        destination.write_text(convert(path.read_text(encoding='utf-8-sig')), encoding='utf-8')
        print(destination.relative_to(ROOT))


if __name__ == '__main__':
    main()
