using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiComponentsLibrary.ViewModels
{
    [Serializable]
    public abstract class DocumentStructure
    {
        public Guid Guid { get; set; }
    }

    [Serializable]
    public class BlockStructure : DocumentStructure
    {
        public Type Type { get; set; }
        public LinkStructure Link { get; set; } = null;
        public List<DocumentStructure> Childrens { get; set; }
    }

    [Serializable]
    public class TextStructure : DocumentStructure
    {
        public string Text { get; set; }
        public bool? IsBold { get; set; }
        public bool? IsItalic { get; set; }
        public bool? IsUnderline { get; set; }
        public bool? IsStrikethrought { get; set; }
        public string FontName { get; set; }
        public float? FontSize { get; set; }
    }

    public class LinkStructure
    {
        public string Url { get; set; }
        public string Caption { get; set; }

    }
}
