# Clinic Management Dashboard (Work in progress)
A clinic management dashboard app that can be controlled through custom data attributes to define how the app will behave and handle different scenarios. 

Backed by OData and .NET6 with MudBlazor for presentation.

- The app relies on the OData batching feature (`$batch`) while tracking changes (CRUD) on data tables, then it batches them all in a single request, parses and displays all the responses in status messages with associated colors for status codes.
- Supports inline editing.
- Full text search using the OData search (`$search`) feature (custom implementation), (`int` and `string` properties by defualt, other types must implement a custom `ToString` override).
- Searchable fields can be controlled through the custom `DataField` attribute, `ServerSearchable` for `$search` and `ClientSearchable` for regular filering through the API.
- All the data tables share the same generic base component that does all the wiring for you.

## Example:
`PatientDTO`:
```csharp
public class PatientDTO : DTOBase
{
    public PatientDTO()
    {
        Gender = Gender.Male;
    }

    [DataField(DisplayName = "First Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public string? FirstName { get; set; }

    [DataField(DisplayName = "Middle Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public string? MiddleName { get; set; }

    [DataField(DisplayName = "Last Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public string? LastName { get; set; }

    [DataField(DisplayName = "Age", DataField = DataField.Number, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public int Age { get; set; }

    [DataField(DisplayName = "Gender", DataField = DataField.Enum, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public Gender Gender { get; set; }

    [DataField(DisplayName = "Phone Number", DataField = DataField.PhoneNumber, Editable = true, ClientSearchable = true, ServerSearchable = true)]
    public string? PhoneNumber { get; set; }

    [DataField(DisplayName = "Extra Data", DataField = DataField.Navigation)]
    public ExtraDataDTO? ExtraData { get; set; }

    [DataField(DisplayName = "Appointments", DataField = DataField.Navigation)]
    public List<AppointmentDTO>? Appointments { get; set; }

    [DataField(DisplayName = "Treatments", DataField = DataField.Navigation)]
    public List<TreatmentDTO>? Treatments { get; set; }

    [DataField(DisplayName = "Notes", DataField = DataField.Navigation)]
    public List<NoteDTO>? Notes { get; set; }
}

```
## Result:
![chrome_Fje6QA0ZWC](https://user-images.githubusercontent.com/38891601/160287557-cfe8c2a4-2818-492f-87f1-fe3dbbd238d4.gif)

## Todo:
### Client:
- Apply colors to modified entities to differentiate which one is edited/deleted before batching changes to server.
- More comprehensive tracking e.g. disable editing for to be deleted entities and vice versa.
- Make status messages associated with enitiy rows in terms of location so we know which message belongs to which entity row.
### Server:
Still thinking...
