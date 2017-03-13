using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EudoxusOsy.Portal.Controls
{
    public abstract class BaseEntityScriptControl<T> : BaseScriptControl where T : class
    {
        public T Entity { get; set; }

        public abstract void Bind();

        public virtual T Extract(T entity) { return entity; }
    }

    public abstract class BaseScriptControl : BaseUserControl, IScriptControl
    {
        protected virtual string ClientControlName
        {
            get { return string.Empty; }
        }

        public virtual string ClientControlPath { get { return string.Empty; } }

        protected override void OnPreRender(EventArgs e)
        {
            if (ScriptManager.GetCurrent(Page) == null)
                throw new NullReferenceException("A ScriptManager control must exist on the Page");
            else
            {
                if (!string.IsNullOrEmpty(ClientControlPath))
                {
                    ScriptReference sr = new ScriptReference(ClientControlPath);
                    if (!ScriptManager.GetCurrent(this.Page).CompositeScript.Scripts.Any(x => x.Path == sr.Path))
                    {
                        ScriptManager.GetCurrent(this.Page).CompositeScript.Scripts.Add(sr);
                    }
                }
                ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
            }
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //if (!string.IsNullOrEmpty(ClientControlPath))
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);
            base.Render(writer);
        }

        #region [ IScriptControl Members ]

        public virtual IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            yield return null;
        }

        public virtual IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return null;
        }

        #endregion
    }
}