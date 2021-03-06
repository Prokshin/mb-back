PGDMP                           x            mb    12.2    12.2 $    1           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            2           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            3           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            4           1262    17545    mb    DATABASE     �   CREATE DATABASE mb WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Russian_Russia.1251' LC_CTYPE = 'Russian_Russia.1251';
    DROP DATABASE mb;
                postgres    false            �            1259    17645    accounts    TABLE     �   CREATE TABLE public.accounts (
    id bigint NOT NULL,
    balance numeric DEFAULT 0,
    user_id integer,
    status character varying DEFAULT 'open'::character varying,
    is_close boolean DEFAULT false
);
    DROP TABLE public.accounts;
       public         heap    postgres    false            �            1259    17658    account_id_seq    SEQUENCE     �   CREATE SEQUENCE public.account_id_seq
    START WITH 4000000000
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.account_id_seq;
       public          postgres    false    204            5           0    0    account_id_seq    SEQUENCE OWNED BY     B   ALTER SEQUENCE public.account_id_seq OWNED BY public.accounts.id;
          public          postgres    false    205            �            1259    17673 
   operations    TABLE     "  CREATE TABLE public.operations (
    id integer NOT NULL,
    date timestamp with time zone NOT NULL,
    operation_type character varying NOT NULL,
    amount numeric NOT NULL,
    account_in_id bigint,
    account_out_id bigint,
    requisite_id integer,
    purpose character varying
);
    DROP TABLE public.operations;
       public         heap    postgres    false            �            1259    17671    operations_id_seq    SEQUENCE     �   CREATE SEQUENCE public.operations_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.operations_id_seq;
       public          postgres    false    207            6           0    0    operations_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.operations_id_seq OWNED BY public.operations.id;
          public          postgres    false    206            �            1259    17702 
   requisites    TABLE     �   CREATE TABLE public.requisites (
    payment_name character varying,
    target_name character varying,
    target_email character varying,
    id integer NOT NULL,
    user_id integer
);
    DROP TABLE public.requisites;
       public         heap    postgres    false            �            1259    17715    requisites_id_seq    SEQUENCE     �   CREATE SEQUENCE public.requisites_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.requisites_id_seq;
       public          postgres    false    208            7           0    0    requisites_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.requisites_id_seq OWNED BY public.requisites.id;
          public          postgres    false    209            �            1259    17556    users    TABLE     �   CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(30),
    email character varying,
    password character varying,
    img character varying,
    is_deleted boolean DEFAULT false
);
    DROP TABLE public.users;
       public         heap    postgres    false            �            1259    17554    users_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    203            8           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    202            �
           2604    17661    accounts id    DEFAULT     i   ALTER TABLE ONLY public.accounts ALTER COLUMN id SET DEFAULT nextval('public.account_id_seq'::regclass);
 :   ALTER TABLE public.accounts ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    205    204            �
           2604    17676    operations id    DEFAULT     n   ALTER TABLE ONLY public.operations ALTER COLUMN id SET DEFAULT nextval('public.operations_id_seq'::regclass);
 <   ALTER TABLE public.operations ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    207    206    207            �
           2604    17717    requisites id    DEFAULT     n   ALTER TABLE ONLY public.requisites ALTER COLUMN id SET DEFAULT nextval('public.requisites_id_seq'::regclass);
 <   ALTER TABLE public.requisites ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    209    208            �
           2604    17559    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    203    202    203            )          0    17645    accounts 
   TABLE DATA           J   COPY public.accounts (id, balance, user_id, status, is_close) FROM stdin;
    public          postgres    false    204   �(       ,          0    17673 
   operations 
   TABLE DATA           |   COPY public.operations (id, date, operation_type, amount, account_in_id, account_out_id, requisite_id, purpose) FROM stdin;
    public          postgres    false    207   �(       -          0    17702 
   requisites 
   TABLE DATA           Z   COPY public.requisites (payment_name, target_name, target_email, id, user_id) FROM stdin;
    public          postgres    false    208   n)       (          0    17556    users 
   TABLE DATA           K   COPY public.users (id, name, email, password, img, is_deleted) FROM stdin;
    public          postgres    false    203   �)       9           0    0    account_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.account_id_seq', 4000000025, true);
          public          postgres    false    205            :           0    0    operations_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.operations_id_seq', 30, true);
          public          postgres    false    206            ;           0    0    requisites_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.requisites_id_seq', 16, true);
          public          postgres    false    209            <           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 15, true);
          public          postgres    false    202            �
           2606    17663    accounts accounts_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT accounts_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.accounts DROP CONSTRAINT accounts_pkey;
       public            postgres    false    204            �
           2606    17681    operations operations_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_pkey;
       public            postgres    false    207            �
           2606    17725    requisites requisites_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.requisites
    ADD CONSTRAINT requisites_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.requisites DROP CONSTRAINT requisites_pkey;
       public            postgres    false    208            �
           2606    17564    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    203            �
           2606    17653    accounts accounts_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT accounts_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
 H   ALTER TABLE ONLY public.accounts DROP CONSTRAINT accounts_user_id_fkey;
       public          postgres    false    204    203    2718            �
           2606    17682 (   operations operations_account_in_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_account_in_id_fkey FOREIGN KEY (account_in_id) REFERENCES public.accounts(id);
 R   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_account_in_id_fkey;
       public          postgres    false    2720    207    204            �
           2606    17687 )   operations operations_account_out_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_account_out_id_fkey FOREIGN KEY (account_out_id) REFERENCES public.accounts(id);
 S   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_account_out_id_fkey;
       public          postgres    false    204    2720    207            �
           2606    17728 "   requisites requisites_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.requisites
    ADD CONSTRAINT requisites_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);
 L   ALTER TABLE ONLY public.requisites DROP CONSTRAINT requisites_user_id_fkey;
       public          postgres    false    203    2718    208            )   $   x�31� #SNCNC����<�4�=... l~d      ,   r   x����	�0��TE��bo�tWD*p��I҂J�B��,��e�� &�	�	"���~�x��g۸�e�:�$x'����v4�}��!Z�4��S�'��G������om.9��^O�      -      x������ � �      (   n   x�34�0�¾��x��~β�����D������RN�DCC���쬔_w����PgC�����RC�Ȋ���b��2����(��t���<#�?�4�=... ~$�     