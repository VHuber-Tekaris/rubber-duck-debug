# Checkpoints

Each folder holds the Docker and Compose files as they should look after one task, to get to the state after a task is finished.

```powershell
.\checkpoints\apply.ps1 2
```

That copies the state after task 2 over your working copy, which puts you at the
start of task 3. Applying a checkpoint **overwrites your own versions of those
files**, so commit or stash anything you want to keep first.

Going *backwards* leaves containers behind for services that no longer exist
  in `compose.yaml`. Compose calls those orphans and does not remove them on its
  own, so use `docker compose up -d --build --remove-orphans` after applying an
  earlier checkpoint.

To get back to the original state of the whole repository instead:

```powershell
git restore .
git clean -fd
```
