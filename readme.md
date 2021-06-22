# REST APIs - Hypermedia and PATCH

## Disclaimer

The code in the repo is provided only to demonstrate concepts.  The implementation is NOT production strength, or even using recommended best practices, it is merely a means to an end to demonstrate hypermedia and the use of the PATCH verb.  So, don't copy the code!  But if you run it, you may find the way the API works interesting.

## Hypermedia

This API uses a sample domain of apprentices and apprenticeships.  It exposes an apprentice resource and apprenticeships as children.  Hypermedia is used to signpost the allowable actions that can be performed on a resource based upon its current state.  These actions are exposed as links within the JSON response for each resource and they represent the Domain Application Protocol (DAP) concerning how the resource can be modified.

## PATCH

The repo also demonstrates the use of the PATCH verb to perform partial updates to resources.  In a recent project, I saw individual attributes of a resource exposed as totally different resources, which a client was expected to POST to  update :-(.  E.g. `/api/apprentice/322/email`
.  When I queried this, I was told they didn't want to do a PUT to `/api/apprentice/322`
 to update the entire representation of the resource, so had exposed email separately.  The issue with this of course (in addition to not being entirely RESTful) is that exposing many attributes in this manner, e.g. `/api/apprentice/322/email`
 and `/api/apprentice/322/dateOfBirth`
 would lead to clients having to call many endpoints with no transaction between them for the updates (i.e. some attributes get updated, others do not).  As a result, I suggested I created a simple example to show how PATCH could be used to perform a partial resource update on the ACTUAL resource `/api/apprentice/322` without the need to expose additional child resources.

JSON Patch is a standard following this [RFC](https://datatracker.ietf.org/doc/html/rfc6902).  Which provides a standard JSON definition regarding how to patch a resource.  The example herein follows that standard.

Handling this within a ASP.NET Core 3.1 API controller is pretty easy, using the `JsonPatchDocument` class e.g.

```c#
[HttpPatch("{Id}", Name = RouteNames.Apprentice)]
public IActionResult PatchApprentice(int id, [FromBody] JsonPatchDocument<Apprentice> apprenticePatch)
{
    var app = ApprenticeCache.Apprentices.Where(a => a.Id == id).FirstOrDefault();
    if (app == default(Apprentice))
        return NotFound();

    // Apply the PATCH instruction to the entity
    apprenticePatch.ApplyTo(app);

    var appR = new ApprenticeResource(app, Url);
    return Ok(appR);
}
```
