# This function allow the game to get a pre-ordened pattern
def pattern(set_columns: int, set_lines: int):
    index = 0
    list_of_patterns = []

    for i in range(set_columns):
        suport_list = []
        index += 1
        
        for j in range(set_lines):
            suport_list.append(index)
        
        list_of_patterns.append(suport_list)
    
    return list_of_patterns