namespace ClinicProject.Shared.Attributes
{
    [Flags]
    public enum DataField
    {
        None = 0,
        Empty = 1,
        Text = 2,
        MultiLineText = 4,
        Number = 8,
        PhoneNumber = 16,
        Currency = 32,
        DateTime = 64,
        Date = 128,
        Time = 256,
        Enum = 512,
        Navigation = 1024,
        NavigationExpanded = 2048,
        TextNavigationExpanded = Text | NavigationExpanded,
        NumberNavigationExpanded = Number | NavigationExpanded,
        CurrencyNavigationExpanded = Currency | NavigationExpanded,
        DateTimeNavigationExpanded = DateTime | NavigationExpanded,
        EnumNavigationExpanded = Enum | NavigationExpanded,
        PhoneNumberNavigationExpanded = PhoneNumber | NavigationExpanded,
    }
}
