# e-tinglysing-afpant-folgebrev-1.0.0
## Følgebrev fra bank til megler - AFPANT

XSD: https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev/afpant-folgebrev-xsd/afpant-folgebrev-1.0.0.xsd

XSLT: https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev/afpant-folgebrev-xslt/afpant-folgebrev-1.0.0.xslt

## Sammendrag
Følgebrev fra bank kan produseres som XML. Dokumentet må da validere i henhold til XSD, og vil bli rendret av mottakers system ved hjelp av XSLT.
Referanse til XSD (xsi:noNamespaceSchemaLocation) og XSLT (<?xml-stylesheet />) må inkluderes i produsert XML slik at dokumentet blir self-contained.

XSD og XSLT vil bli hostet på https://last-opp.pantedokumentet.no/AFPANT/ - korrekt URI annonseres snart.

## Eksempel
XSD.EXE kan brukes for å autogenerere en POCO fra XSD (Se eksempel på https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev/generate-poco-from-xsd.bat)

Hvordan serialisere AFPANT.Folgebrev til XML med XSD+XSLT referanser: https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev-example/Program.cs

![Eksempel på rendret følgebrev som html](https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev/afpant-folgebrev-xslt/afpant-folgebrev-rendered-example.PNG)