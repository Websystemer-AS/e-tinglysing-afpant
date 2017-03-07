# e-tinglysing-afpant-1.0.0
## Sammendrag
Bruker i avsender-bank må innhente hvilket organisasjonsnummer forsendelsen skal til (dette hentes normalt sett ut fra signert kopi av kjøpekontrakt, og er enten organisasjonsnummeret til eiendomsmeglerforetaket eller oppgjørsforetaket).

Deretter produseres det et **ZIP**-arkiv som inneholder:
* Kjøpers pantedokument SDO (1 eller flere filer)
* Eventuelt følgebrev (PDF/XML) (med forutsetninger for oversendelse av pantedokument, evt innbetalingsinformasjon)
* Dersom følgebrev produseres som XML må dokumentet validere i henhold til afpant-folgebrev XSD. 

**NB**: Overførsel av mer enn 1 pantedokument-SDO i samme forsendelse skal normalt sett benyttes for å kunne tinglyse separate pant på hver av hjemmelshavere (men på samme matrikkelenhet). For eksempel i tilfeller hvor det er to debitorer (låntakere) som ikke er ektefeller/samboere/registrerte partnere.

Avsender-bank angir metadata-keys på Altinn-forsendelse (i manifestet) som indikerer om avsender-bank ønsker avlesingskvittering (maskinell og/eller pr email), og hvorvidt følgebrevet er inkludert i ZIP eller om det sendes out-of-band (f.eks via fax eller mail direkte til megler/oppgjør).
Mottaker (systemleverandør) pakker ut ZIP og parser SDO for å trekke ut nøkkeldata (kreditor, debitor(er), matrikkelenhet(er)) som brukes for å rute forsendelsen til korrekt oppdrag hos korrekt megler/oppgjørsforetak.

## Avlesningskvittering
Avsender-bank kan angi hvorvidt mottakende fagsystem skal returnere en avlesningskvittering, og man kan velge følgende metoder:
* Avsender-bank angir i Altinn-metadata keys (senderName/senderEmail/senderPhone) kontaktinformasjonen til kontaktperson i bank og key (notificationMode) om de ønsker emailvarsling fra mottakende fagsystem ved suksessfull ruting og/eller feil.
* Avsender-bank angir i Altinn-metadata key (notificationMode) enum verdi «AltinnNotification». Dette betyr at avsender-bank ønsker en strukturert ack/nack-melding fra mottakende fagsystem ved behandling. Ack/nack-meldingen kan da brukes av avsender-bank til å oppdatere state/workflow i eget (bank)fagsystem.
* Avsender-bank angir i Altinn-metadata key (coverLetter) enum verdi som tilsier hvorvidt følgebrevet ligger som PDF/XML inne i ZIP eller om det sendes til megler/oppgjør på annet vis. Eventuell PDF/XML er ment til manuell behandling av oppgjørsansvarlig på lik linje med dagens papirbaserte følgebrev. 

## Altinn Formidlingstjenester manifest metadata-keys ved innsending fra banksystem til meglersystem
  <table>
        <tbody>
            <tr>
                <td><p><strong>Key</strong></p></td>
                <td><p><strong>Type</strong></p></td>
                <td><p><strong>Required</strong></p></td>
                <td><p><strong>Beskrivelse</strong></p></td>
            </tr>
            <tr>
                <td><p>messageType</p></td>
                <td><p>String</p></td>
                <td><p>Yes</p></td>
                <td><p>Denne kan være en av følgende:</p><ul><li>SignedMortgageDeed</li></ul></td>
            </tr>
            <tr>
                <td><p>notificationMode</p></td>
                <td><p>String[] (enum[])</p></td>
                <td><p>No</p></td>
                <td><p>Dette serialiseres som en json array av alle notifications avsender ønsker. Følgende strenger kan være verdi i array:</p><ul><li>EmailNotificationWhenRoutedSuccessfully</li><li>EmailNotificationWhenFailed</li><li>AltinnNotification</li></ul><p>Hvis du f.eks. har «EmailNotificationWhenFailed» og «AltinnNotification» skal mottaker sende epost hvis behandling av pantedokumentet feiler og uansett sende en ack/nack gjennom Altinn.</p></td>
            </tr>
            <tr>
                <td><p>senderName</p></td>
                <td><p>String</p></td>
                <td><p>No</p></td>
                <td><p>Navn på avsender (mennesket)</p></td>
            </tr>
            <tr>
                <td><p>senderEmail</p></td>
                <td><p>String</p></td>
                <td><p>No</p></td>
                <td><p>Required hvis notificationMode EmailNotificationWhenRoutedSuccessfully eller EmailNotificationWhenFailed er angitt.<br> Email til avsender</p></td>
            </tr>
            <tr>
                <td><p>senderPhone</p></td>
                <td><p>String</p></td>
                <td><p>No</p></td>
                <td><p>Tlf til avsender</p></td>
            </tr>
            <tr>
                <td><p>coverLetter</p></td>
                <td><p>String[] (enum[])</p></td>
                <td><p>Yes</p></td>
                <td><p>Denne kan være en av følgende statuser:</p><ul><li>FileAttached</li><li>SentOutOfBand</li><li>Omitted</li></ul></td>
            </tr>
            <tr>
                <td><p>payload</p></td>
                <td><p>String</p></td>
                <td><p>Yes</p></td>
                <td><p>Base64-encodet streng av ZIP-arkivet.</p></td>
            </tr>
        </tbody>
    </table>

## Manifest metadata-keys ved retur av ACK/NACK notification fra fagsystem til bank (etter behandling av mottatt pantedokument):
    <table>
        <tbody>
            <tr>
                <td><p><strong>Key</strong></p></td>
                <td><p><strong>Type</strong></p></td>
                <td><p><strong>Beskrivelse</strong></p></td>
            </tr>
            <tr>
                <td><p>messageType</p></td>
                <td><p>String</p></td>
                <td><p>Denne kan være en av følgende:</p><ul><li>SignedMortgageDeedProcessed</li></ul></td>
            </tr>
            <tr>
                <td><p>altinnReceiptReference</p></td>
                <td><p>String</p></td>
                <td><p>Altinn kvitteringsreferanse på opprinnelig innsendt pantedokument-SDO fra bank.</p></td>
            </tr>
            <tr>
                <td><p>status</p></td>
                <td><p>String (enum)</p></td>
                <td><p>Denne kan være en av følgende statuser:</p><ul><li>RoutedSuccessfully</li><li>UnknownCadastre</li><li>UnknownCreditor</li><li>UnknownDebitor</li><li>Rejected</li></ul></td>
            </tr>
            <tr>
                <td><p>externalSystemId</p></td>
                <td><p>String</p></td>
                <td><p>Optional: ID/oppdragsnummer/key i eksternt meglersystem/fagsystem.</p></td>
            </tr>
        </tbody>
    </table>