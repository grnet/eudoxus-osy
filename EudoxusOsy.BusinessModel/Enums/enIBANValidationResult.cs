namespace EudoxusOsy.BusinessModel
{
    public enum enIBANValidationResult
    {
        IsValid,
        ContainsSpaces,
        ValueMissing,
        ValueTooSmall,
        ValueTooBig,
        ValueFailsModule97Check,
        CountryCodeNotKnown,
        NotGreekIBAN
    }
}