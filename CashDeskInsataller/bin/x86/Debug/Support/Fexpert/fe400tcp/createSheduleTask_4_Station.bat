schtasks.exe /create /tn "FinExpert_importSettings" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_importSettings.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Import::importSettings\"" /ru ""
schtasks.exe /create /tn "FinExpert_importGCategory" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_importGCategory.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Import::importGlobalCategory\"" /ru ""
schtasks.exe /create /tn "FinExpert_importProfile" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_importProfile.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Import::importProfile\"" /ru ""
schtasks.exe /create /tn "FinExpert_importArtikul" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_importArtikul.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Import::importArtikul\"" /ru ""
schtasks.exe /create /tn "FinExpert_exportCheckAndPos" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_exportCheckAndPos.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Export::exportCheckAndPos\"" /ru ""
schtasks.exe /create /tn "FinExpert_importRefs4Discount" /sc ONCE /st 00:00:01 /tr "c:\FExpert\fe400tcp\FEMFC_importRefs4Discount.exe -UNATTENDED -FeUser:\"cashdesk\" -FePsw:\"cashdesk\" -FeFirm:52 -FeTask:\"SO_Cash\" -Mtd:\"Cashdesk_Import::importRefs4Discount\"" /ru ""