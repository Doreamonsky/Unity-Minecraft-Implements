Obfuscator Copyright (c) 2017-2019 OrangePearSoftware. All Rights Reserved


Usage
=====

OrangePearSoftwares Obfuscator is developed to increase software and game security especially for
games build with Unity3d. It feature is to obfuscated built dotNet assemblies, like
assembly-csharp and assembly-unityscript for the Platforms Windows/Mac/Linux Stan-
dalone, the embedded Platform Android and IPhone and consoles.
Obfuscator Free considers specific Unity features, like MonoBehaviour, NetworkBe-
haviours, Serialization, Refl
ection, and so on, to allow a easy and out of the box working
obfuscator. Obfuscator features reachs from simple renaming:
- Classes
- Fields
- Propertys
- Events
- Methods
Up to Method content obfuscation.

With the Pro Version you have access to:
- Assembly Definition File Obfuscation.
- External Dll obfuscation.
- MonoBehaviour/NetworkBehaviour/ScriptAbleObject obfuscation.
- String obfuscation.
- Protect classes against refactoring!
- And many more features!

Options
=======

From within Unity, select "OPS->Obfuscator Setting".

From the Inspector window, you can now see the Obfuscation options available along with descriptions where relevant. The default settings provide a solid configuration that obfuscates the majority of your code, but here you have general control over what is obfuscated.

Code Attributes
===============

Methods often need to be left unobfuscated so that they can be referenced from an external plugin via reflection, or for some other reason.

You can achieve this by adding Attributes to your code.

The following OrangePearSoftware specific attributes are supported:

[DoNotRenameAttribute]                   - The obfuscator will not rename this class/method/field, but will continue to obfuscate its contents (if relevant).
[DoNotObfuscateClassAttribute]           - The obfuscator will not rename this class, nor will it obfuscate its contents (fields/methods/...).
[DoNotObfuscateMethodBodyAttribute]      - The obfuscator will not rename content in the specific method.

Troubleshooting F.A.Q
=====================

Q. After obfuscating, my 3rd party plugin has stopped working!

A. The simplest way to fix this is to look at the plugin's script to see what namespace they use. Then, in the inspector window in "OPS->Obfuscator Settings" there is an array called "Skip Namespaces". Add the plugin's namespace to this array and the obfuscator will ignore any matching namespaces. Occassionally a plugin will forget to use namespaces for its scripts, in which case you have to : Annotate each class with [OPS.Obfuscator.Attribute.DoNotObfuscateClassAttribute].


Q. Button clicks don't work anymore!

A. Check your Options and see if you enabled the "public methods". If you did, then make sure you've added a [Obfuscator.Attribute.DoNotRenameAttribute] attribute to the button click method.
   For a more obfuscated approach you could assign button clicks programatically. For example, here the ButtonMethod can be obfuscated:

     public Button mybutton;

     public void Start()
     {
       mybutton.onClick.AddListener(ButtonMethod);
     }
  
   The same process works for all gui methods.
   But mostly if you check in the inspector window in "OPS->Obfuscator Settings": Find Gui Methods, it will find many to all!


Q. Animation events don't work anymore!

A. See "Button clicks don't work anymore!". If a method is being typed into the inspector, you should exclude it from Obfuscation.
   Here works also: if you check in the inspector window in "OPS->Obfuscator Settings": Find Animation Methods, it will find many to all!

Q. It's not working for a certain platform.

A. Regardless of the platform, send us an email (orangepearsoftware@gmail.com) with the error and we'll see what we can do.


Q. How can we run obfuscation later in the build process?

A. You can control this in the Assets/ObfuscatorPro/Editor/BuildPostProcessor.cs script. The PostProcessScene attribute on the Obfuscate method has an index number that you can freely change to enable other scripts to be called first.


Q. Can I obfuscate externally created DLLs?

A. Yes, with Obfuscator Pro you can enter under: OPS->Obfuscator Settings->General Assemblies.


Q. How do I obfuscate local variables?

A. Local variable names are not stored anywhere, so there is nothing to obfuscate. A decompiler tries to guess a local variable's name based on the name of its class, or the method that instantiated it.


Q. When I build with Jenkins there is an error: Asset obfuscating finished with ERRORS!

A. When calling BuildPlayer by yourself, through Jenkins for example, some Unity intern events will not get called. So you have to call it yourself.

	Obfuscator.BuildPostProcessor.ManualRestoreUnityObjects();
	BuildPlayer(....)
	Obfuscator.BuildPostProcessor.ManualRestoreUnityObjects();

	
Q. Something's still not working, how can I get in touch with you?

A. Please email us at orangepearsoftware@gmail.com giving as much information about the problem as possible.