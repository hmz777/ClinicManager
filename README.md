# Clinic Management Dashboard (Work in progress)
A clinic management dashboard app that can be controlled through custom data attributes to define how the app will behave and handle different scenarios. 

Backed by OData and .NET6 with MudBlazor for presentation.

- The app relies on the OData batching feature (`$batch`) while tracking changes (CRUD) on data tables, then it batches them all in a single request, parses and displays all the responses in status messages with associated colors for status codes.
- Supports inline editing.
- Full text search using the OData search (`$search`) feature (custom implementation), (`int` and `string` properties by defualt, other types must implement a custom `ToString` override).
- Searchable fields can be controlled through the custom `DataField` attribute, `ServerSearchable` for `$search` and `ClientSearchable` for regular filering through the API.
- All the data tables share the same generic base component that does all the wiring for you.
