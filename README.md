# Sweet Todo List API

A Fun Code exercise for a cool company

## Getting started

Update the connection string in launchSetting.json to your sql server instance. Build, run, and have an awesome time creating, reading, updating and deleting your todos.

### Note on Patch Document for PartiallyUpdateTodo Endpoint

Documentation for using JsonPatch in ASP.NET Core 3.1 can be found here: https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1

Example Body of a request:

```
[
  {
    "op": "replace",
    "path": "/name",
    "value": "Best Todo Ever"
  },
  {
    "op": "replace",
    "path": "/description",
    "value": "You will remember this one forever."
  }
]
```
