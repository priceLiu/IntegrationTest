<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="922e63f4-c0b6-4ac8-9884-86948aee941a" namespace="IntegrationTest" xmlSchemaNamespace="urn:IntegrationTest" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="TestSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="testSection">
      <elementProperties>
        <elementProperty name="Cases" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="cases" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/TestCaseConfCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="TestCaseConf">
      <attributeProperties>
        <attributeProperty name="Sleep" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="sleep" isReadOnly="false" defaultValue="1000">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="RunTime" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="runTime" isReadOnly="false" defaultValue="10">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Count" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="count" isReadOnly="false" defaultValue="10000">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxThread" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="maxThread" isReadOnly="false" defaultValue="1">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Units" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="units" isReadOnly="false" defaultValue="10">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Counters" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="counters" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/TestCounterConfCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Params" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="params" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/ParamCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="TestCaseConfCollection" collectionType="AddRemoveClearMapAlternate" xmlItemName="testCaseConf" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/TestCaseConf" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="TestCounterConf">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="From" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="from" isReadOnly="false" defaultValue="0">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="To" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="to" isReadOnly="false" defaultValue="1000">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Count" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="count" isReadOnly="false" defaultValue="0">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="TestCounterConfCollection" collectionType="AddRemoveClearMapAlternate" xmlItemName="testCounterConf" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/TestCounterConf" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ParamCollection" xmlItemName="param" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/Param" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="Param">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/922e63f4-c0b6-4ac8-9884-86948aee941a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>