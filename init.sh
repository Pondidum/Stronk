#!/bin/sh

# we might not have a git directory yet
mkdir -p ".git/hooks"

if [[ -n "$WINDIR" ]]; then
    # windows not happy about symbolic links
    find hooks/ -type f -exec sh -c 'ln --force ./{} .git/hooks/$(basename {})' \;
else
    find hooks/ -type f -exec sh -c 'ln -s --force ./{} .git/hooks/$(basename {})' \;
fi

echo 'hooks installed.'
