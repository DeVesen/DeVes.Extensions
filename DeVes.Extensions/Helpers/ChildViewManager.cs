using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace DeVes.Extensions.Helpers
{
    public class ChildViewManager
    {
        private readonly IContainer m_container;
        private readonly List<ICvItem> m_activeChilds = new List<ICvItem>();


        public Func<Type, int, bool> AllowSecondInst;
        public Action<ICvItem> ViewActivate;
        public Action<ICvItem> ViewShow;


        public ChildViewManager(params KeyValuePair<Type, Type>[] types)
        {
            var _builder = new ContainerBuilder();

            foreach (var _pair in types)
            {
                _builder.RegisterType(_pair.Key).As(_pair.Value);
            }

            m_container = _builder.Build();
        }
        public ChildViewManager(IContainer container)
        {
            m_container = container;
        }


        public void ShowView<T>(ICviParameter parameter = null)
        {
            ShowView(typeof(T), parameter);
        }
        public void ShowView(Type viewType, ICviParameter parameter = null)
        {
            if (!viewType.IsTypeOf(typeof(ICvItem)))
                return;

            lock (m_activeChilds)
            {
                var _activeItems = GetActivesByType(viewType);
                var _activeItem = _activeItems.FirstOrDefault();

                if (AllowSecondInst != null)
                {
                    if (_activeItems.Length > 0 && !AllowSecondInst(viewType, _activeItems.Length))
                    {
                        ViewActivate?.Invoke(_activeItem);
                        return;
                    }
                }

                var _lts = m_container.BeginLifetimeScope();
                {
                    _activeItem = _lts.Resolve(viewType) as ICvItem;

                    if (_activeItem == null) return;

                    _activeItem.CviClosed = item =>
                    {
                        lock (m_activeChilds)
                        {
                            m_activeChilds.Remove(_activeItem);
                        }
                    };

                    m_activeChilds.Add(_activeItem);
                    ViewShow?.Invoke(_activeItem);
                }
            }
        }


        public ICvItem[] GetActivesByType(Type viewType)
        {
            return m_activeChilds.Where(p => p.GetType().IsTypeOf(viewType)).ToArray();
        }
    }

    public interface ICvItem
    {
        Action<ICvItem> CviClosed { get; set; }

        void PostConstruct(ICviParameter parameter);
    }

    public interface ICviParameter
    {

    }
}
