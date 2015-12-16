# TfvcBranchPolicy ![Build Status](https://nkdagility.visualstudio.com/DefaultCollection/_apis/public/build/definitions/56105d4f-9725-48e5-bf58-fdad743d0c52/34/badge)

The TFVC Branch Policy tool is an atempt to replicate the new funcationality for Git in Visual Studio ALM. This Checkin Policy will allow administrators to enter a regular repression that matches a path, preferrably a branch, in TFVC and applies configured policies to that branch.

Current policies:

1. Check all Checkins for linked work items
2. Require a Code Review be associated with a Checkin
3. Check for bypass string in comment if branch is locked

Future policies:

1. Path specific reviewers
2. Enforce minimum number of reviewrs







