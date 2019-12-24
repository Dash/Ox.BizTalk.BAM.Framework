# Ox.BizTalk.BAM.Framework
This solution provides a framework for the BizTalk BAM API.  It contains some friendly wrappers to the API which makes implementing activities easy through the use of typed classes and properties.

# Dependencies
Ox.BizTalk.BAM top level project is dependent on Microsoft.BizTalk.Bam.EventOrchestration.dll, which is provided as part of the BizTalk install and is freely redistributale within your organisation providing you have a BizTalk license.  This is a standalone DLL.

Ox.BizTalk.BAM.OrchestrationStream relies on Microsoft.BizTalk.Bam.XLANGs.dll, which is also provided as part of a BizTalk install, but is not for redistribution.  You won't need this DLL unless you're consuming the BAM API from within an Orchestration.  This chains on to a lot of other DLLs.

In order to build this code, you will need a copy of BizTalk Server installed on your build machine.  Don't forget to sign it and GAC it if you're planning on consuming from BizTalk Server.

# Usage
1. Extend Ox.BizTalk.BAM.ActivityBase and add the properties that correspond to the BAM Activity that needs to be written.
2. Inject the relevant EventStream (Bufffered,Direct).
3. Call your desired BAM actions.

You can use the ActivityFactory to generate a new instance of your activity class with the desired event stream.

To use OrchestrationEventStream with this framework you will need to use Ox.BizTalk.BAM.OrchestrationEventStreamWrapper to make it compatible with BaseActivity.

## Declaring an activity

Match your BAM activity structure with a POCO class

```csharp
using Ox.BizTalk.BAM;

[BamActivity(Name="My Activity")]   // Optional if class name isn't compatible
public class MyActivity : ActivityBase
{
    public MyActivity() : base() {}

    public MyActivity(IEventStreamMediator eventStreamMediator, string activityId = null) : base(eventStreamMediator, activityId) {}

    public string Field1 { get; set;}

    // Using the NullifyDefaultValues setting will transparently convert a
    // DateTime with the default value to NULL when inserting into the DB
    // Useful as BizTalk doesn't work well with Nullable<T> types
    [BamField(NullifyDefaultValues = true)]
    public DateTime Field2 { get; set; }

    // Use the FieldName setting if required to name the backend field
    [BamField(FieldName = "3rd Field")]
    public string Field3 { get; set; }
}
```

## Consuming your activity

Instantiate a copy of your activity 

```csharp
// To call from an Orchestration
myActivity = new MyActivity(new Ox.BizTalk.BAM.EventStreamMediator(new Microsoft.BizTalk.Bam.EventObservation.OrchestrationEventStream()));

// Or use a factory elsewhere
myActivity = ActivityFactory.NewBufferedActivity<MyActivity>(connectionString);
```

Then call your desired actions
```csharp
myActivity.Field1 = "What's up doc?";
myActivity.BeginActivity();
myActivity.CommitActivity();
myActivity.EndActivity();
```

There are other functions available, such as the ability to add a reference to another activity, and enabling continuations.

The above examples use automatically generated Activity Ids, but if you're working with continuations then you may want to provide a custom one, this can either be done via the constructor, factory or via setting the ```ActivityId``` property directly (but only once eh?).

## Cleaning up
As most of the event streams supplied by BizTalk implement IDisposable, so does this.  You can either call .Dispose() on your activity, or wrap it up in a using() statement.

# Bundled Example
The included example shows you a simple implementation of the BAM API with the accompanying Excel workbook.
It is not intended to be a tutorial on how to use BAM full stop, just how to use this framework.  You will need a preexisting understanding in how to use BAM.

1. Deploy the BAM Activity from the spreadsheet  
```bm deploy-all -DefinitionFile:ExampleWorkbook.xml```
2. Ensure the user you are executing the example under has BAM_EVENT_WRITER role in the BAMPrimaryImport database
3. Run the Program
4. Verify data has arrived in the BAM database (or portal if you setup a view of your own)

# Copyright

Copyright © 2019. Alastair Grant.
https://www.aligrant.com/

Licensed under the [MIT](LICENSE.txt) license.