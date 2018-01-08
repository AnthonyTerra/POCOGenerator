using System;

namespace Db.POCOIterator
{
    public interface IEFIterator : IPOCOIterator
    {
        bool IsEF { get; set; }
        bool IsEFColumn { get; set; }
        bool IsEFRequired { get; set; }
        bool IsEFRequiredWithErrorMessage { get; set; }
        bool IsEFConcurrencyCheck { get; set; }
        bool IsEFStringLength { get; set; }
        bool IsEFDisplay { get; set; }
        bool IsEFDescription { get; set; }
        bool IsEFComplexType { get; set; }
        bool IsEFIndex { get; set; }
        bool IsEFForeignKey { get; set; }
    }
}
