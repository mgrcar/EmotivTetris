# Mindtris

## Notes

I developed/tested this with Epoc+ Premium. I downloaded the latest community SDK and used `edk.dll` that comes with this SDK. 
I had Xavier Control Panel v3.0.0.41 installed on my computer and I did not update that. I trained the four mental commands with this software
and I loaded my [offline] training profile then with the SDK. Note that this stunt only works with the x86 built (because Xavier Control Panel is 
32-bit, I guess). In this setting, detecting mental commands with the SDK was not working nearly as well as in Xavier Control Panel.
My guess is that this is because of one of these two things:

1. `edk.dll` provided with the community SDK is incompatible with my headset's firmware.

1. Profile files, created with Xavier Control Panel v3.0.0.41, are incompatible with the community SDK.

Rather than exploring these two options, I decided to use `edk.dll` and `edk_utils.dll` from the Emotiv premium libraries (v3.0.0.41) that I have
also installed on my computer back then. I modified the C# SDK a bit to make this work. In this new setting, detecting mental commands
works much better.

## Dependencies

To make this work, you need to clone my fork of the SDK (branch `dev`) into the same parent folder:

```
git clone -b dev https://github.com/mgrcar/EmotivSdk.git
```