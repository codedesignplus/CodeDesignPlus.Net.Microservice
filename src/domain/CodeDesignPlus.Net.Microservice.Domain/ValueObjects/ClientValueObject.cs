using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

public sealed partial class ClientValueObject
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string TypeDocument { get; private set; }

    [JsonConstructor]
    private ClientValueObject(Guid id, string name, string document, string typeDocument)
    {
        this.Id = id;
        this.Name = name;
        this.Document = document;
        this.TypeDocument = typeDocument;
    }

    public static ClientValueObject Create(Guid id, string name, string document, string typeDocument)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdClientIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameClientIsInvalid);
        DomainGuard.IsNullOrEmpty(document, Errors.DocumentIsNull);
        DomainGuard.IsNullOrEmpty(typeDocument, Errors.TypeDocumentIsNull);

        return new ClientValueObject(id, name, document, typeDocument);
    }
}