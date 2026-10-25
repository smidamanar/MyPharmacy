namespace MyPharmacy.Api.Services;

public sealed class DomainValidationException(string message) : Exception(message);

public sealed class ResourceNotFoundException(string message) : Exception(message);

public sealed class InventoryConcurrencyException : Exception
{
    public InventoryConcurrencyException()
        : base("Inventory changed while this prescription was being processed. Please retry.") { }
}
