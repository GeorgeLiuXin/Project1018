using UnityEngine;
using System;
using System.Collections.Generic;

namespace XWorld
{
    public class Reference
    {
        public List<Item> callers = new List<Item>();
        public List<Item> references = new List<Item>();
        public bool toggleCallers = true;
        public bool toggleReferences = true;
        public DateTime callerTime = DateTime.Now;
        public DateTime referenceTime = DateTime.Now;

        public void AddCaller(object obj)
        {
            var item = FindItem(this.callers, obj);

            if (item != null)
            {
                item.Retain();
            }
            else
            {
                this.callers.Add(new Item(obj));
            }

            callerTime = DateTime.Now;
        }

        public void RemoveCaller(object obj)
        {
            var item = FindItem(this.callers, obj);

            if (item != null)
            {
                this.callers.Remove(item);

                callerTime = DateTime.Now;
            }
        }

        public void ClearCaller()
        {
            this.callers.Clear();
        }

        public void Retain(object obj)
        {
            var item = FindItem(this.references, obj);

            if (item != null)
            {
                item.Retain();
            }
            else
            {
                this.references.Add(new Item(obj));
            }

            referenceTime = DateTime.Now;
        }

        public void Release(object obj)
        {
            var item = FindItem(this.references, obj);

            if (item != null)
            {
                item.refCount--;
                if (item.refCount > 0)
                {
                    item.Release();
                }
                else
                {
                    this.references.Remove(item);
                }

                referenceTime = DateTime.Now;
            }
        }

        private Item FindItem(List<Item> items, object obj)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetObject() == obj)
                {
                    return items[i];
                }
            }

            return null;
        }

        public class Item
        {
            public string path;
            public string type;
            public WeakReference weakReference;
            public DateTime time;
            public int refCount;

            public Item(object obj)
            {
                if (obj != null)
                {
                    if (obj is GameObject)
                    {
                        var go = obj as GameObject;
                        this.path = Utility.GetHierarchyPath(go);
                    }
                    else if (obj is Component)
                    {
                        var com = obj as Component;
                        this.path = Utility.GetHierarchyPath(com.transform);
                    }
                    else
                    {
                        this.path = obj.ToString();
                    }

                    this.type = obj.GetType().ToString();
                    this.weakReference = new WeakReference(obj);
                }

                UpdateTime();
                Retain();
            }

            public bool IsAlive()
            {
                return this.weakReference.IsAlive;
            }

            public object GetObject()
            {
                if (this.weakReference.IsAlive)
                {
                    return this.weakReference.Target;
                }

                return null;
            }

            public void Retain()
            {
                this.refCount++;
                UpdateTime();
            }

            public void Release()
            {
                this.refCount--;
                UpdateTime();
            }

            private void UpdateTime()
            {
                this.time = DateTime.Now;
            }
        }
    }
}
