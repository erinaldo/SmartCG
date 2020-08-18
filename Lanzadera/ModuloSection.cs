using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SmartCG
{
    class ModuloSection : ConfigurationSection
    {    
        [ConfigurationProperty("modulos")]
        public ModuloCollection Modulos
        {
            get
            { return (ModuloCollection)this["modulos"]; }
            set
            { this["modulos"] = value; }
        }
    }



    public class ModuloElement : ConfigurationElement, IComparable
    {
        public ModuloElement()
        {
        }

        public ModuloElement(string id, string nombre, string moduloNamespace, string formulario, string imagen, int activo, int orden)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Namespace = moduloNamespace;
            this.Formulario = formulario;
            this.Imagen = imagen;
            this.Activo = activo;
            this.Orden = orden;
        }

        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get
            { return (string)this["id"]; }
            set
            { this["id"] = value; }
        }

        [ConfigurationProperty("nombre", IsRequired = true)]
        public string Nombre
        {
            get
            { return (string)this["nombre"]; }
            set
            { this["nombre"] = value; }
        }

        [ConfigurationProperty("namespace", IsRequired = true)]
        public string Namespace
        {
            get
            { return (string)this["namespace"]; }
            set
            { this["namespace"] = value; }
        }

        [ConfigurationProperty("formulario", IsRequired = true)]
        public string Formulario
        {
            get
            { return (string)this["formulario"]; }
            set
            { this["formulario"] = value; }
        }

        [ConfigurationProperty("imagen", IsRequired = true)]
        public string Imagen
        {
            get
            { return (string)this["imagen"]; }
            set
            { this["imagen"] = value; }
        }

        [ConfigurationProperty("orden", IsRequired = true)]
        public int Orden
        {
            get
            { return (int)this["orden"]; }
            set
            { this["orden"] = value; }
        }

        [ConfigurationProperty("activo", IsRequired = true)]
        public int Activo
        {
            get
            { return (int)this["activo"]; }
            set
            { this["activo"] = value; }
        }

        int IComparable.CompareTo(object obj)
        {
            ModuloElement temp = (ModuloElement)obj;
            if (this.Orden > temp.Orden)
                return 1;
            if (this.Orden < temp.Orden)
                return -1;
            else
                return 0;
        }
    }

    
    [ConfigurationCollectionAttribute(typeof(ModuloElement))]
    public class ModuloCollection  : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuloElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuloElement)element).Id;
        }

        public ModuloElement this[int idx]
        {
            get
            {
                return (ModuloElement)BaseGet(idx);
            }
        }
    }





    public class Modulo1 : ConfigurationElement, IComparable
    {
        public Modulo1()
        {
        }

        public Modulo1(string id, string nombre, string nombreDll, string formulario, string imagen, int basico, int activo, int orden)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.NombreDll = nombreDll;
            this.Formulario = formulario;
            this.Imagen = imagen;
            this.Basico = basico;
            this.Activo = activo;
            this.Orden = orden;
        }

        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get
            { return (string)this["id"]; }
            set
            { this["id"] = value; }
        }

        [ConfigurationProperty("nombre", IsRequired = true)]
        public string Nombre
        {
            get
            { return (string)this["nombre"]; }
            set
            { this["nombre"] = value; }
        }

        [ConfigurationProperty("nombreDll", IsRequired = true)]
        public string NombreDll
        {
            get
            { return (string)this["nombreDll"]; }
            set
            { this["nombreDll"] = value; }
        }

        [ConfigurationProperty("formulario", IsRequired = true)]
        public string Formulario
        {
            get
            { return (string)this["formulario"]; }
            set
            { this["formulario"] = value; }
        }

        [ConfigurationProperty("imagen", IsRequired = true)]
        public string Imagen
        {
            get
            { return (string)this["imagen"]; }
            set
            { this["imagen"] = value; }
        }

        [ConfigurationProperty("orden", IsRequired = true)]
        public int Orden
        {
            get
            { return (int)this["orden"]; }
            set
            { this["orden"] = value; }
        }

        [ConfigurationProperty("basico", IsRequired = true)]
        public int Basico
        {
            get
            { return (int)this["basico"]; }
            set
            { this["basico"] = value; }
        }

        [ConfigurationProperty("activo", IsRequired = true)]
        public int Activo
        {
            get
            { return (int)this["activo"]; }
            set
            { this["activo"] = value; }
        }

        int IComparable.CompareTo(object obj)
        {
            ModuloElement temp = (ModuloElement)obj;
            if (this.Orden > temp.Orden)
                return 1;
            if (this.Orden < temp.Orden)
                return -1;
            else
                return 0;
        }
    }


    [ConfigurationCollectionAttribute(typeof(Modulo1))]
    public class Modulo1Coleccion : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Modulo1();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Modulo1)element).Id;
        }

        public Modulo1 this[int idx]
        {
            get
            {
                return (Modulo1)BaseGet(idx);
            }
        }
    }
}
