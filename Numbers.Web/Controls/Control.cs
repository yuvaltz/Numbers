using System;
using System.Collections;
using System.Collections.Generic;
using System.Html;
using System.Linq;

namespace Numbers.Web.Controls
{
    public class Control : IEnumerable, IDisposable
    {
        public Element HtmlElement { get; private set; }

        private int left;
        public int Left
        {
            get { return left; }
            set
            {
                left = value;
                HtmlElement.Style.Left = String.Format("{0}px", left);
            }
        }

        private int top;
        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                HtmlElement.Style.Top = String.Format("{0}px", top);
            }
        }

        private List<Control> children;
        public IEnumerable<Control> Children { get { return children; } }

        public Control(params string[] classesName)
        {
            HtmlElement = Document.CreateElement("div");
            children = new List<Control>();

            foreach (string className in classesName)
            {
                if (!String.IsNullOrEmpty(className))
                {
                    HtmlElement.ClassList.Add(className);
                }
            }
        }

        public void AppendChild(Control child)
        {
            children.Add(child);
            HtmlElement.AppendChild(child.HtmlElement);
        }

        public void InsertChild(int index, Control child)
        {
            children.Insert(index, child);

            if (index < HtmlElement.Children.Length)
            {
                HtmlElement.InsertBefore(child.HtmlElement, HtmlElement.Children[index]);
            }
            else
            {
                HtmlElement.AppendChild(child.HtmlElement);
            }
        }

        public void RemoveChild(Control child)
        {
            children.Remove(child);
            HtmlElement.RemoveChild(child.HtmlElement);
        }

        public int ChildIndex(Control child)
        {
            return children.IndexOf(child);
        }

        public Control ChildAt(int index)
        {
            return children.Where(child => child.HtmlElement == HtmlElement.Children[index]).FirstOrDefault();
        }

        public void Add(object item)
        {
            AppendChild((Control)item);
        }

        public IEnumerator GetEnumerator()
        {
            return null;
        }

        public virtual void Dispose()
        {
            foreach (Control child in Children)
            {
                child.Dispose();
            }
        }
    }
}
