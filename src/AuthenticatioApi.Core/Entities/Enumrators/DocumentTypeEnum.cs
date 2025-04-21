using System.ComponentModel;

namespace AuthenticatioApi.Core.Entities.Enumrators
{
    public enum DocumentTypeEnum
    {
        [Description("CPF")]
        CPF = 1,

        [Description("CNPJ")]
        CNPJ = 2,
    }
}
