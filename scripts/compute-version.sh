#!/usr/bin/env bash
# CalVer generator: YYYY.(MM*100+DD).BUILD
#
#   major = calendar year            e.g. 2026
#   minor = month*100 + day          e.g. Sep 9 -> 909, Jan 5 -> 105
#   build = 1 for the day's first release, then +1 per additional same-day build
#
# The build segment is derived from existing git tags (<major>.<minor>.*), so
# nothing needs to be committed or hand-incremented. Prints just the version
# string to stdout for the release workflow to consume; diagnostics go to stderr
# so VERSION=$(scripts/compute-version.sh) stays clean.
#
# Tags here are un-prefixed (2026.909.1), matching this repo's existing tag
# history and the release workflow's tag_name — dev-prompt prefixes them with a
# leading `v`, this is the one intentional deviation.
set -euo pipefail

major=$(date -u +%Y)
month=$(date -u +%m)
day=$(date -u +%d)
# 10# forces base-10 so a leading zero (08, 09) isn't read as octal.
minor=$(( 10#$month * 100 + 10#$day ))

build=1
tags=$(git tag --list "${major}.${minor}.*" || true)
if [ -n "$tags" ]; then
  highest=$(printf '%s\n' "$tags" \
    | sed -n "s/^${major}\.${minor}\.\([0-9][0-9]*\)$/\1/p" \
    | sort -n | tail -1)
  if [ -n "$highest" ]; then
    build=$(( highest + 1 ))
  fi
fi

version="${major}.${minor}.${build}"
echo "computed version: ${version}" >&2
printf '%s' "$version"
