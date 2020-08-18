using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SmartCG
{
    class IdiomaSection : ConfigurationSection
    {    
        [ConfigurationProperty("idiomas")]
        public IdiomaCollection Idiomas
        {
            get
            { return (IdiomaCollection)this["idiomas"]; }
            set
            { this["idiomas"] = value; }
        }
    }



    public class IdiomaElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get
            { return (string)this["id"]; }
            set
            { this["id"] = value; }
        }

        [ConfigurationProperty("descripcion", IsRequired = true)]
        public string Descripcion
        {
            get
            { return (string)this["descripcion"]; }
            set
            { this["descripcion"] = value; }
        }

        [ConfigurationProperty("cultura", IsRequired = true)]
        public string Cultura
        {
            get
            { return (string)this["cultura"]; }
            set
            { this["cultura"] = value; }
        }

        /*
        [ConfigurationProperty("imagen", IsRequired = true)]
        public string Imagen
        {
            get
            { return (string)this["imagen"]; }
            set
            { this["imagen"] = value; }
        }
        */

        [ConfigurationProperty("activo", IsRequired = true)]
        public int Activo
        {
            get
            { return (int)this["activo"]; }
            set
            { this["activo"] = value; }
        }
    }


    
    [ConfigurationCollectionAttribute(typeof(IdiomaElement))]
    public class IdiomaCollection  : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IdiomaElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IdiomaElement)element).Id;
        }

        public IdiomaElement this[int idx]
        {
            get
            {
                return (IdiomaElement)BaseGet(idx);
            }
        }
    }
}
