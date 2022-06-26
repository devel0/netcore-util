#!/bin/bash

exdir="$(dirname `readlink -f "$0"`)"

DOCSDIR="$exdir/docs"

cd "$exdir"

doxygen

rsync -arvx "$exdir/test/" "$DOCSDIR/test/" \
    --exclude=bin \
    --exclude=obj