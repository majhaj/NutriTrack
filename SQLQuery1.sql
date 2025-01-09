SELECT 
    fk.name AS ForeignKeyName, 
    tp.name AS ParentTable, 
    tr.name AS ReferencedTable
FROM 
    sys.foreign_keys AS fk
INNER JOIN 
    sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables AS tr ON fk.referenced_object_id = tr.object_id
WHERE 
    tp.name = 'Meals' AND tr.name = 'AspNetUsers';
