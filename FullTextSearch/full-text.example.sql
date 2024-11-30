select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring java mvc ultimation') -- khớp Spring Java MVC Ultimated từ Z tới A

-- phraseto_tsquery('spring a an ultimated') thì giữa spring, ultimated có 3 khoảng trống (cụ thể 'spring' <3> 'ultim')
-- nên sẽ khớp 'spring' <-> 'java' <-> 'mvc' <-> 'ultim' <-> 'từ' <-> 'z' <-> 'tới'
select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring a an ultimated') -- khớp Spring Java MVC Ultimated từ Z tới A

-- vì phraseto_tsquery('spring a an từ') có kết quả là 'spring' <3> 'từ'
-- và select phraseto_tsquery('spring a an từ') có kết quả 'spring' <3> 'từ'
select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring a an từ') -- khớp Java Spring REST Ultimate từ A tới Z

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('mvc ultimating') -- khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('  ultimating') -- loại bỏ khoảng trống và vẫn khớp

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ ; Z ; tới  ;  A') -- loại bỏ and ; . , và vẫn khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ ;    Z tới A') -- khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ an ;    Z tới A') -- không khớp Spring Java MVC Ultimated từ Z tới A

-- Query 1: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <2> 'z' <-> 'tới'
-- Query 2: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <7> 'z' <-> 'tới'
-- Query 3: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <4> 'z' <-> 'tới'
select phraseto_tsquery('and , .    ultimating từ ;    Z tới A'), phraseto_tsquery('and , .    ultimating từ an ;    Z tới A');
select phraseto_tsquery('and , .    ultimating từ ; ; :   Z tới A'), phraseto_tsquery(', .    ultimating từ an an a a the a;    Z tới A');
select phraseto_tsquery('and , .    ultimating từ ; ; :   Z tới A'), phraseto_tsquery('the a an the or and , .    ultimating từ an the a;    Z tới A');

-- 2 câu query sau đều có cùng kết quả: 'ultim' & 'từ' & 'z' & 'tới'
select websearch_to_tsquery('and , .    ultimating từ ;    Z tới A'), websearch_to_tsquery('and , .    ultimating từ an ;    Z tới A');
select websearch_to_tsquery('and , .    ultimating từ ; ; :   Z tới A'), websearch_to_tsquery('and , .    ultimating từ an the a;    Z tới A');

-- websearch_to_tsquery tìm kiếm mềm mại hơn so với phraseto_tsquery vì nó không yêu cầu thứ tự
select * from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('and , .    ultimating từ and the or Z tới A') -- 

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('A      tới Z spring mvc ultimation') -- vẫn khớp Spring Java MVC Ultimated từ Z tới A

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('Z tới A spring mvc ultimation') -- khớp Spring Java MVC Ultimated từ Z tới A

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('từ Z spring mvc ultimation') -- vẫn khớp Spring Java MVC Ultimated từ Z tới A