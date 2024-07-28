create database ochlocracy;

create table users (
  user_id bigserial primary key,
  given_name varchar(50) not null,
  family_name varchar(50) not null,
  created_date timestamp default now()
);

create table bill_reaction (
  bill_reaction_id bigserial primary key,
  bill_number varchar(50) not null,
  user_id bigserial references users (user_id),
  explanation varchar(5000) null,
  opinion varchar(5000) null
);
