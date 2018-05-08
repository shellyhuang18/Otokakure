import os;

notes = ['a', 'a#', 'b', 'c', 'c#', 'd', 'd#', 'e', 'f', 'f#', 'g', 'g#']
octave = 0

itr = 0

folder = os.listdir('.')
folder = sorted(folder)
for file in folder:
	if(file.endswith('.wav')):
		new_name = notes[itr] + str(octave) + '.wav'
		os.rename(file, new_name)
		itr += 1
		if(itr == 3):
			octave += 1
		if(itr == 12):
			itr = 0



