using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EudoxusOsy.Portal.Controls
{
    public class BaseEntityUserControl<T> : UserControl where T : class
    {
        public T Entity { get; set; }

        /// <summary>
        /// Binds the given <see cref="Entity"/> to the control
        /// </summary>
        public virtual void Bind() { }

        /// <summary>
        /// Rebinds the Entity to the control. Is intended to use when you want to support a diferrent type of Bind scenario. ex:Not override some values
        /// </summary>
        public virtual void ReBind() { Bind(); }

        /// <summary>
        /// Fills the entity with the appropriate values
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Fill(T entity) { return entity; }

        public virtual T Fill() { return Fill(Entity); }


    }

    public class BaseEntityUserControl<TEntity, TPage> : BaseEntityUserControl<TEntity>
        where TPage : Page
        where TEntity : class
    {
        public new TPage Page { get { return (TPage)base.Page; } }
    }
}
