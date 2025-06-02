create database ochlocracy;

create table users (
  user_id bigserial primary key,
  username varchar(320) not null,
  given_name varchar(50) not null,
  family_name varchar(50) not null,
  created_at timestamp default now()
);

create table bill_opinions (
  bill_opinion_id bigserial primary key,
  bill_number varchar(50) not null,
  user_id bigserial references users (user_id),
  opinion varchar(5000) null,
  score integer default 0
);

create table bill_explanations (
  bill_explanation_id bigserial primary key,
  bill_number varchar(50) not null,
  user_id bigserial references users (user_id),
  explanation varchar(5000) null,
  score integer default 0
);

insert into users (username, given_name, family_name) values
  ('test@test.com', 'michael', 'cuomo');


do $$
declare u_id bigint;
begin
  u_id := (select u.user_id from users u where u.username = 'test@test.com' limit 1);

  insert into bill_opinions (bill_number, user_id, opinion, score) values
    ('bill_number_1', u_id, 'opinion 1', 0),
    ('bill_number_1', u_id, 'opinion 2', 0),
    ('bill_number_1', u_id, 'opinion 3', 0),
    ('bill_number_2', u_id, 'opinion 2', 0);

  insert into bill_explanations (bill_number, user_id, explanation, score) values
    ('bill_number_1', u_id, 'explanation 1', 0),
    ('bill_number_1', u_id, 'explanation 2', 0),
    ('bill_number_1', u_id, 'explanation 3', 0),
    ('bill_number_2', u_id, 'explanation 2', 0);

end 
$$

