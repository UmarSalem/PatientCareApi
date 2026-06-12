# Git Workflow Notes

This project uses feature branches and pull requests.

## Branch Strategy

```text
main = stable final branch
development = integration branch
feature/* = one feature at a time
```

Feature branches should normally be created from `development`.

## Merge

Merge combines one branch into another and keeps the branch history.

Example:

```powershell
git checkout development
git merge feature/05-api-controllers
```

On GitHub, clicking **Merge pull request** does this for you.

### When To Use Merge

Use merge when:

- the PR is already pushed/shared
- you want to preserve branch history
- you are merging a completed feature into `development`

Interview explanation:

> Merge keeps the real history of how branches came together. It is safe for shared branches because it does not rewrite commit history.

## Rebase

Rebase moves your feature branch commits so they appear on top of the latest target branch.

Example:

```powershell
git checkout feature/05-api-controllers
git fetch origin
git rebase origin/development
```

This creates a cleaner, straight-line history.

### When To Use Rebase

Use rebase when:

- you are cleaning up your own local feature branch
- you want your feature branch updated with latest `development`
- the branch is not shared, or your team agrees rewriting is okay

Interview explanation:

> Rebase rewrites the feature branch history so it looks like the work started from the latest development branch. It gives a clean history, but it should be used carefully on shared branches.

## Merge vs Rebase

| Concept | Merge | Rebase |
| --- | --- | --- |
| Keeps branch history | Yes | No, rewrites feature history |
| Creates merge commit | Usually yes | No |
| Best for shared branches | Yes | Be careful |
| Best for clean local history | Sometimes | Yes |

## Safe Rule For This Project

Use this before starting a new feature:

```powershell
git checkout development
git pull origin development
git checkout -b feature/new-feature-name
```

Use this when your feature branch needs latest development before pushing:

```powershell
git checkout feature/new-feature-name
git fetch origin
git rebase origin/development
```

If you already pushed the branch and opened a PR, prefer GitHub merge unless you are comfortable with force push.

## If Rebase Has Conflicts

Fix the files, then run:

```powershell
git add .
git rebase --continue
```

Cancel the rebase:

```powershell
git rebase --abort
```

## If You Rebases A Pushed Branch

Only if needed, push with:

```powershell
git push --force-with-lease
```

`--force-with-lease` is safer than `--force` because it checks whether someone else pushed changes first.
