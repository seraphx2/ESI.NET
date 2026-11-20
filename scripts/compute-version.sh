#!/usr/bin/env bash
# Version generator: YYYY.(MM*100+DD).BUILD, from the ESI compatibility date.
#
#   major = year of the pinned compatibility date          e.g. 2026
#   minor = month*100 + day of that date                   e.g. 2026-11-19 -> 1119
#   build = 1 for the first release on that date, then +1 per release after it
#
# The date is ESI.NET.EsiVersion.CompatibilityDate - the X-Compatibility-Date
# header every request sends - so the version names the ESI contract the package
# speaks: a new year/minor means the package moved to a newer dated snapshot of
# ESI (ESI only issues new dates for breaking changes), a new build is library
# work on the same contract. Compatibility dates only move forward, so versions
# stay monotonic. Releases before 2026.1119.1 were stamped from the release date
# instead; never publish a date older than the newest existing tag.
#
# The build segment is derived from existing git tags (<major>.<minor>.*), so
# nothing needs to be committed or hand-incremented. Prints just the version
# string to stdout for the release workflow to consume; diagnostics go to stderr
# so VERSION=$(scripts/compute-version.sh) stays clean.
#
# Tags here are un-prefixed (2026.1119.1), matching this repo's existing tag
# history and the release workflow's tag_name.
set -euo pipefail

root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
source_file="${root}/ESI.NET/EsiVersion.cs"

compat=$(sed -n 's/.*CompatibilityDate *= *"\([0-9]\{4\}-[0-9]\{2\}-[0-9]\{2\}\)".*/\1/p' "$source_file")
if [ -z "$compat" ]; then
  echo "compute-version: no CompatibilityDate found in ${source_file}" >&2
  exit 1
fi

IFS=- read -r major month day <<< "$compat"
# 10# forces base-10 so a leading zero (08, 09) isn't read as octal.
minor=$(( 10#$month * 100 + 10#$day ))

build=1
tags=$(git -C "$root" tag --list "${major}.${minor}.*" || true)
if [ -n "$tags" ]; then
  highest=$(printf '%s\n' "$tags" \
    | sed -n "s/^${major}\.${minor}\.\([0-9][0-9]*\)$/\1/p" \
    | sort -n | tail -1)
  if [ -n "$highest" ]; then
    build=$(( highest + 1 ))
  fi
fi

version="${major}.${minor}.${build}"

# Guard: a package version must never sort below one already published (a
# compatibility date older than a date-of-release tag from the old scheme, or a
# rolled-back date). Compares against the newest plain X.Y.Z release tag.
newest=$(git -C "$root" tag --list \
  | grep -E '^[0-9]+\.[0-9]+\.[0-9]+$' \
  | sort -t. -k1,1n -k2,2n -k3,3n | tail -1 || true)
if [ -n "$newest" ]; then
  lowest=$(printf '%s\n%s\n' "$newest" "$version" | sort -t. -k1,1n -k2,2n -k3,3n | head -1)
  if [ "$lowest" = "$version" ] && [ "$version" != "$newest" ]; then
    echo "compute-version: ${version} would sort below the newest release ${newest}" >&2
    exit 1
  fi
fi

echo "computed version: ${version} (compatibility date ${compat})" >&2
printf '%s' "$version"
