using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
using EPiServer.Personalization;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.Util.PlugIns;
using System.Web.UI;
using EPiServer.UI;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.WebForms;
using EPiServer;

namespace Nergard.EPi.Plugins
{
    [GuiPlugIn(DisplayName = "Missing Properties", Description = "Missing Properties", Area = PlugInArea.AdminMenu, Url = "~/Plugins/MissingProperties.aspx")]
    public partial class MissingProperties :  WebFormsBase
    {
        protected class Result
        {
            public string Heading { get; set; }
            public string TypeName { get; set; } 
            public string Name { get; set; }
            public int id { get; set; }
        }

        private ContentTypeModelRepository _contentTypeModelRepository;
        List<Result> _results = new List<Result>();

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
            this.SystemMessageContainer.Heading = "Missing properties";
            this.SystemMessageContainer.Description = "Lists all misssing properties on page- and blocktypes and gives the possibility to delete them";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this._contentTypeModelRepository = ServiceLocator.Current.GetInstance<ContentTypeModelRepository>();
            Setup();
        }

        private void Setup()
        {
            CheckTypes();
            PropertiesViewControl.DataSource = _results;
            PropertiesViewControl.DataBind();           
        }

        private void CheckTypes()
        {
            int x = 0;

            foreach (var type in this.PageTypeRepository.List())
            {

                foreach (var property in type.PropertyDefinitions)
                {
                    if (IsMissingModelProperty(property))
                    {
                        x++;
                        _results.Add(new Result { Heading = (x == 1 ? "Page types" : ""), TypeName=type.Name, Name = property.Name,id= property.ID });
                    }
                }
            }

            x = 0;
            foreach (var btype in this.BlockTypeRepository.List())
            {
                foreach (var bproperty in btype.PropertyDefinitions)
                {
                    if (IsMissingModelProperty(bproperty))
                    {
                        x++;
                        _results.Add(new Result { Heading =( x == 1 ? "Block types" : ""),TypeName = btype.Name,  Name = bproperty.Name, id = bproperty.ID});
                    }
                }
            }

        }

        private bool IsMissingModelProperty(PropertyDefinition propertyDefinition)
        {
            return (((propertyDefinition != null) && propertyDefinition.ExistsOnModel) && (this._contentTypeModelRepository.GetPropertyModel(propertyDefinition.ContentTypeID, propertyDefinition) == null));
        }

        protected void Delete(object sender, EventArgs e)
        {
            IPropertyDefinitionRepository repository = ServiceLocator.Current.GetInstance<IPropertyDefinitionRepository>();

            foreach (GridViewRow row in PropertiesViewControl.Rows)
            {
                bool delete = ((CheckBox)row.FindControl("box")).Checked;
                string typeId = ((Label)row.FindControl("typeid")).Text;

                if (delete)
                {
                    repository.Delete(repository.Load(Int32.Parse(typeId)).CreateWritableClone());
                }
            }

            base.Response.Redirect("MissingProperties.aspx");

        }

        protected EPiServer.DataAbstraction.PageTypeRepository PageTypeRepository
        {
            get
            {
                return (new EPiServer.DataAbstraction.PageTypeRepository(this.ContentTypeRepository));
            }
        }

        protected EPiServer.DataAbstraction.BlockTypeRepository BlockTypeRepository
        {
            get
            {
                return (new EPiServer.DataAbstraction.BlockTypeRepository(this.ContentTypeRepository));
            }
        }


        protected Result Item { get { return Page.GetDataItem() as Result; } }

 

    }
}