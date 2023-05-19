import os
from bin2img import *

input_path = 'F:/out/dv2_uncompressed/'  # Replace with the directory path you want to list
output_path = 'F:/out/dv2_converted/'

files = os.listdir(input_path)
for file in files:
    file_path = os.path.join(input_path, file)
    if os.path.isfile(file_path):
        try:
            dimensions = file_path[-11:][:7]
            width = int(dimensions[:3])
            height = int(dimensions[-3:])
            file_name = file[:-4] + ".png"
            print(dimensions + " " + "W: " + str(width) + " H: " + str(height))
            binary_to_image(file_path, width, height, output_path + file_name)
            print(file)
        except ValueError:
            print(ValueError)