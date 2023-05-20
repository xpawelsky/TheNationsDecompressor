import struct
from PIL import Image

def binary_to_image(inputPath, width, height, outputPath):
    with open(inputPath, 'rb') as f:
        binary_data = f.read()

    pixel_data = []
    for i in range(0, len(binary_data), 2):
        # Read two bytes from the binary data
        pixel = struct.unpack('<H', binary_data[i:i+2])[0]

        # Convert the pixel value to 4-bit components (RGBA 4444 format)
        r = (pixel >> 8) & 0xF
        g = (pixel >> 4) & 0xF
        b = pixel & 0xF

        # Scale the 4-bit components to 8-bit range (0-255)
        r = r * 17
        g = g * 17
        b = b * 17

        pixel_data.append((r, g, b))

    try:
        image = Image.new('RGB', (width, height))
        image.putdata(pixel_data)
        image.save(outputPath, 'PNG')
    except Exception as e:
        print(e)