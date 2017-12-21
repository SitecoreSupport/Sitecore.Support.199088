namespace Sitecore.Support
{
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Publishing.Pipelines.PublishItem;
  public class CloneLayoutPublisher
  {
    public void Process(PublishItemContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      using (new SecurityModel.SecurityDisabler())
      {
        Item itemToPublish = context.PublishHelper.GetItemToPublish(context.ItemId);

        Item itemFromWeb;
        Item itemFromMaster;
        if (itemToPublish != null)
        {
          itemFromWeb = Database.GetDatabase("web").GetItem(context.ItemId, itemToPublish.Language);
          itemFromMaster = Database.GetDatabase("master").GetItem(context.ItemId, itemToPublish.Language);
        }
        else
        {
          itemFromWeb = Database.GetDatabase("web").GetItem(context.ItemId);
          itemFromMaster = Database.GetDatabase("master").GetItem(context.ItemId);
        }

        
        if (itemFromWeb!=null && itemFromMaster!=null && itemFromMaster.IsClone)
        {
          var finalLayoutFromMaster = LayoutField.GetFieldValue(itemFromMaster.Fields[FieldIDs.FinalLayoutField]);
          itemFromWeb.Editing.BeginEdit();
          LayoutField.SetFieldValue(itemFromWeb.Fields[FieldIDs.LayoutField], finalLayoutFromMaster);
          itemFromWeb.Editing.EndEdit();
        }
      }
    }
  }
}