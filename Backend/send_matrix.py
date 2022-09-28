class Matrix:
    def __init__(self, horizontal_lines: int, vertical_lines: int):
        self.h = horizontal_lines
        self.v = vertical_lines
    
    def structure(self):
        horizontal = list()
        for i in range(self.h):
            vertical = list()
            counter = 0
            
            for j in range(self.v):
                counter += 1
                vertical.append(counter)
            
            horizontal.append(vertical)
        
        new_matrix = horizontal
        return new_matrix