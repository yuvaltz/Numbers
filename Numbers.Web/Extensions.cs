using System;
using System.Html;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Numbers.Web
{
    [IgnoreNamespace]
    [Imported]
    public class ComputedStyle
    {
        public string GetPropertyValue(string name)
        {
            return null;
        }
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("window")]
    public static class WindowExtensions
    {
        public static ComputedStyle GetComputedStyle(Element element)
        {
            return null;
        }

        public static int RequestAnimationFrame(Action callback)
        {
            return 0;
        }

        [InlineCode("'requestAnimationFrame' in window")]
        public static bool IsRequestAnimationFrameSupported()
        {
            return false;
        }

        [InlineCode("'ontouchstart' in window")]
        public static bool IsTouchAvailable()
        {
            return false;
        }
    }

    public static class StyleExtensions
    {
        [InlineCode("{style}.transition")]
        public static string GetTransition(this Style style)
        {
            return null;
        }

        [InlineCode("{style}.transition = {value}")]
        public static void SetTransition(this Style style, string value)
        {
            //
        }

        public static TokenDictionary GetTransitionDictionary(this Style style)
        {
            return new TokenDictionary(() => GetTransition(style), value => SetTransition(style, value));
        }
    }

    public static class ElementExtensions
    {
        [InlineCode("'ontouchstart' in {element}")]
        public static bool IsTouchAvailable(this Element element)
        {
            return false;
        }
    }

    public static class EventExtensions
    {
        [InlineCode("{e}.srcElement")]
        public static EventTarget GetSrcElement(this Event e)
        {
            return null;
        }
    }

    public static class ElementCollectionExtensions
    {
        public static Element[] ToArray(this ElementCollection collection)
        {
            Element[] array = new Element[collection.Length];

            for (int i = 0; i < collection.Length; i++)
            {
                array[i] = collection[i];
            }

            return array;
        }
    }

    public static class DocumentExtensions
    {
        public static string GetMetaPropertyValue(string propertyName)
        {
            return Document.GetElementsByTagName("meta").ToArray().Where(element => element.GetAttribute("property") == propertyName).First().GetAttribute("content");
        }
    }
}
